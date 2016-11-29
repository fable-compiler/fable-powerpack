#r "../node_modules/fable-core/Fable.Core.dll"
#r "../npm/Fable.PowerPack.dll"

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Fable.PowerPack.Result

let inline equal (expected: 'T) (actual: 'T): unit =
    let assert' = importAll<obj> "assert"
    assert'?equal(actual, expected) |> ignore

[<Global>]
let it (msg: string) (f: unit->JS.Promise<'T>): unit = jsNative

// Fetch polyfill for node
JsInterop.importAll "isomorphic-fetch"

it "fetch: requests work" <| fun () ->
    let getWebPageLength url =
        fetch url []
        |> Promise.bind (fun res -> res.text())
        |> Promise.map (fun txt -> txt.Length)
    getWebPageLength "http://fable.io"
    |> Promise.map (fun res -> res > 0 |> equal true)

it "fetch: parallel requests work" <| fun () ->
    let getWebPageLength url =
        promise {
            let! res = fetch url []
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

it "fetch: unsuccessful HTTP status codes throw an error" <| fun () ->
    promise {
        let! res = fetch "http://fable.io/this-must-be-an-invalid-url-no-really-i-mean-it" []
        let! txt = res.text()
        return txt
    }
    |> Promise.catch (fun e -> e.Message)
    |> Promise.map (fun results ->
        results |> equal "404 Not Found for URL http://fable.io/this-must-be-an-invalid-url-no-really-i-mean-it")

it "tryFetch: unsuccessful HTTP status codes returns an Error" <| fun () ->
    tryFetch "http://fable.io/this-must-be-an-invalid-url-no-really-i-mean-it" []
    |> Promise.map (fun a ->
        match a with
        | Ok a -> "failed"
        | Error e -> e.Message)
    |> Promise.map (fun results ->
        results |> equal "404 Not Found for URL http://fable.io/this-must-be-an-invalid-url-no-really-i-mean-it")

it "tryFetch: exceptions return an Error" <| fun () ->
    tryFetch "http://this-must-be-an-invalid-url-no-really-i-mean-it.com" []
    |> Promise.map (fun a ->
        match a with
        | Ok a -> "failed"
        | Error e -> e.Message)
    |> Promise.map (fun results ->
        results |> equal "request to http://this-must-be-an-invalid-url-no-really-i-mean-it.com failed, reason: getaddrinfo ENOTFOUND this-must-be-an-invalid-url-no-really-i-mean-it.com this-must-be-an-invalid-url-no-really-i-mean-it.com:80")
