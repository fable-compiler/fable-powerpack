module FetchTests

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Fable.PowerPack.Result

let inline equal (expected: 'T) (actual: 'T): unit =
    Testing.Assert.AreEqual(expected, actual)

[<Global>]
let it (msg: string) (f: unit->JS.Promise<'T>): unit = jsNative

[<Global>]
let describe (msg: string) (f: unit->unit): unit = jsNative

// Fetch polyfill for node
JsInterop.importAll "isomorphic-fetch"

describe "Fetch tests" <| fun _ ->
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

    it "tryFetch: Successful HTTP OPTIONS request" <| fun () ->
        let successMessage = "OPTIONS request accepted (method allowed)"
        let props = [ RequestProperties.Method HttpMethod.OPTIONS]
        tryFetch "http://mockbin.org/bin/042d3a8c-edee-40ae-80f3-70ece6ae247e/view" props 
        |> Promise.map (fun a ->
            match a with
            | Ok _ -> successMessage
            | Error e -> e.Message)
        |> Promise.map (fun results ->
            results |> equal successMessage)

    it "tryFetch: Failed HTTP OPTIONS request on Fable.io server" <| fun () ->
        let failMessage = "OPTIONS request rejected (method not allowed)"
        let props = [ RequestProperties.Method HttpMethod.OPTIONS]
        tryFetch "http://fable.io" props 
        |> Promise.map (fun a ->
            match a with
            | Ok a -> "ohoh? Fable.io allows OPTIONS request?"
            | Error e -> failMessage)
        |> Promise.map (fun results ->
            results |> equal failMessage)

    it "tryFetch: exceptions return an Error" <| fun () ->
        tryFetch "http://this-must-be-an-invalid-url-no-really-i-mean-it.com" []
        |> Promise.map (fun a ->
            match a with
            | Ok a -> "failed"
            | Error e -> e.Message)
        |> Promise.map (fun results ->
            results |> equal "request to http://this-must-be-an-invalid-url-no-really-i-mean-it.com failed, reason: getaddrinfo ENOTFOUND this-must-be-an-invalid-url-no-really-i-mean-it.com this-must-be-an-invalid-url-no-really-i-mean-it.com:80")
