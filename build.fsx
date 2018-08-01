#r "paket: groupref netcorebuild //"
#load ".fake/build.fsx/intellisense.fsx"
#if !FAKE
#r "netstandard"
#r "Facades/netstandard"
#endif

#nowarn "52"

#load "paket-files/netcorebuild/fsharp/FAKE/modules/Octokit/Octokit.fsx"
#load "paket-files/netcorebuild/fable-compiler/fake-helpers/Fable.FakeHelpers.fs"

open System
open System.IO
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open Fake.Tools.Git
open Fake.JavaScript

open Fable.FakeHelpers
open Octokit

let project = "fable-powerpack"
let gitOwner = "fable-compiler"

let versionFromGlobalJson : DotNet.CliInstallOptions -> DotNet.CliInstallOptions = (fun o ->
        { o with Version = DotNet.Version (DotNet.getSDKVersionFromGlobalJson()) }
    )

let dotnetSdk = lazy DotNet.install versionFromGlobalJson
let inline dtntWorkDir wd =
    DotNet.Options.lift dotnetSdk.Value
    >> DotNet.Options.withWorkingDirectory wd
let inline dtntSmpl arg = DotNet.Options.lift dotnetSdk.Value arg

let CWD = __SOURCE_DIRECTORY__

// Clean and install dotnet SDK
Target.create "Bootstrap" (fun _ ->
    !! "src/bin"
    ++ "src/obj"
    ++ "tests/bin"
    ++ "tests/obj" |> Shell.cleanDirs
)

Target.create "Restore" (fun _ ->
    DotNet.restore (dtntWorkDir (CWD </> "tests")) ""
    Yarn.install (fun o -> { o with WorkingDirectory = CWD })
)

Target.create "Test" (fun _ ->
    let result = DotNet.exec (dtntWorkDir CWD) "fable" "webpack-cli -- --config webpack.test.config.js"

    if not result.OK then failwithf "Build of tests project failed."

    Yarn.exec "mocha build" (fun o -> { o with WorkingDirectory = CWD })
)

let docFsproj = "./docs/Docs.fsproj"
let docs = CWD </> "docs"
let docsContent = docs </> "src" </> "Content"
let buildMain = docs </> "build" </> "src" </> "Main.js"

let buildSass _ =
    Yarn.exec "run node-sass --output-style compressed --output docs/public/ docs/scss/main.scss" id

let applyAutoPrefixer _ =
    Yarn.exec "run postcss docs/public/main.css --use autoprefixer -o docs/public/main.css" id

Target.create "Docs.Watch" (fun _ ->
    use watcher = new FileSystemWatcher(docsContent, "*.md")
    watcher.IncludeSubdirectories <- true
    watcher.EnableRaisingEvents <- true

    watcher.Changed.Add(fun _ ->
        Process.execSimple
            (fun info ->
                { info with
                    FileName = "node"
                    Arguments = buildMain }
            )
            (TimeSpan.FromSeconds 30.) |> ignore
    )

    // Make sure the style is generated
    // Watch mode of node-sass don't trigger a first build
    buildSass ()

    [ async {
        let result = DotNet.exec (dtntWorkDir CWD) "fable" "yarn-run fable-splitter -- -c docs/splitter.config.js -w"

        if not result.OK then failwithf "Build of tests project failed."
      }
      async {
        Yarn.exec "run node-sass --output-style compressed --watch --output docs/public/ docs/scss/main.scss" id
      }
    //   async {
    //     Yarn.exec "run http-server -c-1 docs/public" id
    //   }
    ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

Target.create "Docs.Setup" (fun _ ->
    // Make sure directories exist
    Directory.ensure "./docs/scss/extra/highlight.js/"
    Directory.ensure "./docs/public/demos/"

    // Copy files from node_modules allow us to manage them via yarn
    Shell.copyDir "./docs/public/fonts" "./node_modules/font-awesome/fonts" (fun _ -> true)
    Shell.copyFile "./docs/scss/extra/highlight.js/atom-one-light.css" "./node_modules/highlight.js/styles/atom-one-light.css"

    DotNet.restore (dtntWorkDir (CWD </> "docs")) ""
)

Target.create "Docs.Build" (fun _ ->
    let result = DotNet.exec (dtntWorkDir CWD) "fable" "yarn-run fable-splitter -- -c docs/splitter.config.js -p"

    if not result.OK then failwithf "Build of tests project failed."

    buildSass ()
    applyAutoPrefixer ()
)

Target.create "PublishPackages" (fun _ ->
    let sdk = dotnetSdk.Value (DotNet.Options.Create())
    [ "src/Fable.PowerPack.fsproj" ]
    |> publishPackages CWD sdk.DotNetCliPath
)

Target.create "GitHubRelease" (fun _ ->
    let releasePath = CWD </> "src/RELEASE_NOTES.md"
    githubRelease releasePath gitOwner project (fun user pw release ->
        createClient user pw
        |> createDraft gitOwner project release.NugetVersion
            (release.SemVer.PreRelease <> None) release.Notes
        |> releaseDraft
        |> Async.RunSynchronously
    )
)

// Where to push generated documentation
let githubLink = "https://github.com/fable-compiler/fable-powerpack.git"
let publishBranch = "gh-pages"
let temp = CWD </> "temp"

Target.create "Docs.Publish" (fun _ ->
    // Clean the repo before cloning this avoid potential conflicts
    Shell.cleanDir temp
    Repository.cloneSingleBranch "" githubLink publishBranch temp

    // Copy new files
    Shell.copyRecursive "docs/public" temp true |> printfn "%A"

    // Deploy the new site
    Staging.stageAll temp
    Commit.exec temp (sprintf "Update site (%s)" (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
    Branches.push temp
)

"Bootstrap"
    ==> "Restore"
    ==> "Test"
    ==> "PublishPackages"
    ==> "GitHubRelease"

"Docs.Setup"
    <== [ "Restore" ]

"Docs.Build"
    <== [ "Docs.Setup" ]

"Docs.Watch"
    <== [ "Docs.Setup" ]

"Docs.Build"
    ==> "Docs.Publish"

Target.runOrDefault "Test"
