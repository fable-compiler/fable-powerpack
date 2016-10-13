// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open System
open System.IO

let projName = "Fable.PowerPack"
let npmPkgName = "fable-powerpack"
let release = ReleaseNotesHelper.LoadReleaseNotes "RELEASE_NOTES.md"

module Util =
    let run workingDir fileName args =
        printfn "CWD: %s" workingDir
        let fileName, args =
            if EnvironmentHelper.isUnix
            then fileName, args else "cmd", ("/C " + fileName + " " + args)
        let ok =
            execProcess (fun info ->
                info.FileName <- fileName
                info.WorkingDirectory <- workingDir
                info.Arguments <- args) TimeSpan.MaxValue
        if not ok then failwith (sprintf "'%s> %s %s' task failed" workingDir fileName args)

Target "Clean" (fun _ ->
    CleanDirs ["npm"]
)

Target "Build" (fun _ ->
    !! ("src" </> (projName + ".fsproj"))
    |> MSBuild "npm" "Build" [
        "Configuration", "Release"
        "DebugSymbols", "False"
        "DebugType", "None"
        "DocumentationFile", ".." </> "npm" </> (projName + ".xml")
    ]
    |> Log "Build-Release: "

    // Remove dlls except for the project one, can this be set in MSBuild?
    !! "npm/*.dll"
    |> Seq.iter (fun file ->
        if file.Contains(projName) |> not then
            FileUtils.rm file
    )

    // Compile to JS
    Util.run "." "fable" ""

    // Copy README and package.json
    FileUtils.cp "README.md" "npm"
    FileUtils.cp "package.json" "npm"

    // Update version
    Util.run "npm" "npm" ("version " + release.NugetVersion)
)

Target "Test" (fun () ->
    let devTestPkg = "build/tests/node_modules" </> npmPkgName
    CleanDir "build/tests"

    // Copy development files to the package folder in build/tests
    CreateDir devTestPkg
    !! "npm/*.js" |> Seq.iter (fun file -> FileUtils.cp file devTestPkg)

    // Tests will be run by fableconfig postbuild script    
    Util.run "." "fable" "tests/"
)

Target "All" DoNothing

"Clean"
  ==> "Build"
  ==> "Test"
  ==> "All"

// start build
RunTargetOrDefault "All"
