// include Fake libs
#r "./packages/build/FAKE/tools/FakeLib.dll"
#r "System.IO.Compression.FileSystem"

#load "paket-files/build/fable-compiler/fake-helpers/Fable.FakeHelpers.fs"

open System
open Fake
open Fake.YarnHelper
open Fake.Git

let dotnetcliVersion = "2.0.0"
let packages = ["Fable.PowerPack"]

let mutable dotnetExePath = "dotnet"
let runDotnet dir =
    DotNetCli.RunCommand (fun p -> { p with ToolPath = dotnetExePath
                                            WorkingDir = dir } )

Target "InstallDotNetCore" (fun _ ->
   dotnetExePath <- DotNetCli.InstallDotNetSDK dotnetcliVersion
)

Target "Clean" (fun _ ->
    !! "bin"
    ++ "obj"
    |> CleanDirs
)

Target "Install" (fun _ ->
    runDotnet "" "restore"
)

Target "InstallYarn" (fun _ ->
    Yarn (fun p ->
            { p with
                Command = Install Standard
            })
)

Target "Build" (fun _ ->
    runDotnet "" "build"
)

Target "CleanTests" (fun _->
    !! "tests/bin"
    ++ "tests/obj"
    |> CleanDirs
)

Target "InstallTests" (fun _ ->
    runDotnet "tests" "restore"
)

Target "BuildTests" (fun _ ->
    runDotnet "tests" "build"
)

Target "RunTests" (fun _ ->
    runDotnet "" "fable npm-run test --port free"
)

Target "CleanDocs" (fun _ ->
  seq [
    "docs/_public"
    "docs/.sass-cache"
  ] |> CleanDirs
)

// --------------------------------------------------------------------------------------
// Generate the documentation
let githubLink = "git@github.com:fable-compiler/fable-powerpack.git"
let publishBranch = "gh-pages"
let fableRoot   = __SOURCE_DIRECTORY__
let temp        = fableRoot </> "temp"
let docsDir = fableRoot </> "docs"
let docsOuput = docsDir </> "_public"
let fornax = "fornax"

Target "BuildDocs" (fun _ ->
    Fable.FakeHelpers.run docsDir fornax "build"
)

// --------------------------------------------------------------------------------------
// Release Scripts

Target "PublishPackage" (fun _ ->
  Fable.FakeHelpers.publishPackages __SOURCE_DIRECTORY__ dotnetExePath packages
)

Target "PublishDocs" (fun _ ->
  CleanDir temp
  Repository.cloneSingleBranch "" githubLink publishBranch temp

  CopyRecursive docsOuput temp true |> tracefn "%A"
  StageAll temp
  Git.Commit.Commit temp (sprintf "Update site (%s)" (DateTime.Now.ToShortDateString()))
  Branches.push temp
)

"Clean"
    ==> "InstallDotNetCore"
    ==> "Install"
    ==> "Build"
    ==> "InstallYarn"
    ==> "CleanTests"
    ==> "InstallTests"
    ==> "BuildTests"
    ==> "RunTests"

"RunTests"
    ==> "PublishPackage"

// Build order
"CleanDocs"
    ==> "BuildDocs"
    ==> "PublishDocs"

// start build
RunTargetOrDefault "Build"
