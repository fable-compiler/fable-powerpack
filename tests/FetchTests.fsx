#r "../node_modules/fable-core/Fable.Core.dll"
#r "../npm/Fable.PowerPack.dll"

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.PowerPack
open Fable.PowerPack.Fetch

let inline equal (expected: 'T) (actual: 'T): unit =
    let assert' = importAll<obj> "assert"
    assert'?equal(actual, expected) |> ignore

[<Global>]
let it (msg: string) (f: unit->JS.Promise<'T>): unit = jsNative

// Fetch polyfill for node
JsInterop.importAll "isomorphic-fetch"

it "Parallel fetch requests work" <| fun () ->
    let getWebPageLength url =
        fetch(url, [])
        |> Promise.bind (fun res -> res.text())
        |> Promise.map (fun txt -> txt.Length)
    getWebPageLength "http://fable.io"
    |> Promise.map (fun res -> res > 0 |> equal true)

it "Parallel fetch requests work" <| fun () ->
    let getWebPageLength url =
        promise {
            let! res = fetch(url, [])
            let! txt = res.text()
            return txt.Length
        }
    [ "http://fable-compiler.github.io"
    ; "http://babeljs.io"
    ; "http://fsharp.org" ]
    |> List.map getWebPageLength
    |> Promise.Parallel
    // The sum of lenghts of all web pages is
    // expected to be bigger than 100 characters
    |> Promise.map (fun results -> (Array.sum results) > 100 |> equal true)
