// include Fake libs
#r "./packages/build/FAKE/tools/FakeLib.dll"
#r "System.IO.Compression.FileSystem"

open System
open System.IO
open Fake
open Fake.NpmHelper
open Fake.Git

Target "Clean" (fun _ ->
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

// Build order
"Clean"
    ==> "BuildDocs"
    ==> "PublishDocs"

// start build
RunTargetOrDefault "BuildDocs"
