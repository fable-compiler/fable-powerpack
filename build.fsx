// include Fake libs
#r "./packages/build/FAKE/tools/FakeLib.dll"
#r "System.IO.Compression.FileSystem"

open System
open System.IO
open Fake
open Fake.YarnHelper
open Fake.Git

let dotnetcliVersion = "2.0.0"

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
let args = "build"

Target "BuildDocs" (fun _ ->
    let fileName, args =
        if EnvironmentHelper.isUnix
        then fornax, args else "cmd", ("/C " + fornax + " " + args)

    let ok =
        execProcess (fun info ->
            info.FileName <- fileName
            info.WorkingDirectory <- docsDir
            info.Arguments <- args) TimeSpan.MaxValue
    if not ok then failwith (sprintf "'%s> %s %s' task failed" docsDir fileName args)
)

// --------------------------------------------------------------------------------------
// Release Scripts

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


// Build order
"CleanDocs"
    ==> "BuildDocs"
    ==> "PublishDocs"

// start build
RunTargetOrDefault "Build"
