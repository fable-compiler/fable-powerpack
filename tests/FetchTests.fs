module FetchTests

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Fable.PowerPack.Result
open Thoth.Json

let inline equal (expected: 'T) (actual: 'T): unit =
    Testing.Assert.AreEqual(expected, actual)

[<Global>]
let it (msg: string) (f: unit->JS.Promise<'T>): unit = jsNative

[<Global>]
let describe (msg: string) (f: unit->unit): unit = jsNative

module RandomUser =

    type UserName =
        { Title : string
          First : string
          Last : string }

        static member Decoder =
            Decode.object (fun get ->
                { Title = get.Required.Field "title" Decode.string
                  First = get.Required.Field "first" Decode.string
                  Last = get.Required.Field "last" Decode.string } : UserName
            )

    type UserDob =
        { Date : string
          Age : int }

        static member Decoder =
            Decode.object (fun get ->
                { Date = get.Required.Field "date" Decode.string
                  Age = get.Required.Field "age" Decode.int } : UserDob
            )

    type User =
        { Gender : string
          Name : UserName
          Dob : UserDob }

        static member Decoder =
            Decode.object (fun get ->
                { Gender = get.Required.Field "gender" Decode.string
                  Name = get.Required.Field "name" UserName.Decoder
                  Dob = get.Required.Field "dob" UserDob.Decoder } : User
            )

    type ApiInfo =
        { Seed : string
          Results : int
          Page : int
          Version : string }

        static member Decoder =
            Decode.object (fun get ->
                { Seed = get.Required.Field "seed" Decode.string
                  Results = get.Required.Field "results" Decode.int
                  Page = get.Required.Field "page" Decode.int
                  Version = get.Required.Field "version" Decode.string } : ApiInfo
            )

    type ApiResult =
        { Results : User list
          Info : ApiInfo }

        static member Decoder =
            Decode.object (fun get ->
                { Results = get.Required.Field "results" (Decode.list User.Decoder)
                  Info = get.Required.Field "info" ApiInfo.Decoder } : ApiResult
            )

describe "Fetch tests" <| fun _ ->
    it "fetch: requests work" <| fun () ->
        let getWebPageLength url =
            fetch url []
            |> Promise.bind (fun res -> res.text())
            |> Promise.map (fun txt -> txt.Length)
        getWebPageLength "https://fable.io"
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
            let! res = fetch "https://fable.io/this-must-be-an-invalid-url-no-really-i-mean-it" []
            let! txt = res.text()
            return txt
        }
        |> Promise.catch (fun e -> e.Message)
        |> Promise.map (fun results ->
            results |> equal "404 Not Found for URL https://fable.io/this-must-be-an-invalid-url-no-really-i-mean-it")

    it "tryFetch: unsuccessful HTTP status codes returns an Error" <| fun () ->
        tryFetch "https://fable.io/this-must-be-an-invalid-url-no-really-i-mean-it" []
        |> Promise.map (fun a ->
            match a with
            | Ok a -> "failed"
            | Error e -> e.Message)
        |> Promise.map (fun results ->
            results |> equal "404 Not Found for URL https://fable.io/this-must-be-an-invalid-url-no-really-i-mean-it")

    it "tryFetch: Successful HTTP OPTIONS request" <| fun () ->
        let successMessage = "OPTIONS request accepted (method allowed)"
        let props = [ RequestProperties.Method HttpMethod.OPTIONS]
        tryFetch "https://gandi.net" props
        |> Promise.map (fun a ->
            match a with
            | Ok _ -> successMessage
            | Error e -> e.Message)
        |> Promise.map (fun results ->
            results |> equal successMessage)

    it "tryFetch: Failed HTTP OPTIONS request on Fable.io server" <| fun () ->
        let failMessage = "OPTIONS request rejected (method not allowed)"
        let props = [ RequestProperties.Method HttpMethod.OPTIONS]
        tryFetch "https://fable.io" props
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

    it "tryOptionsRequest: Successful HTTP OPTIONS request" <| fun () ->
        let successMessage = "OPTIONS request accepted (method allowed)"
        tryOptionsRequest "https://gandi.net"
        |> Promise.map (fun a ->
            match a with
            | Ok _ -> successMessage
            | Error e -> e.Message)
        |> Promise.map (fun results ->
            results |> equal successMessage)

    it "tryOptionsRequest: Failed HTTP OPTIONS request on Fable.io server" <| fun () ->
        let failMessage = "OPTIONS request rejected (method not allowed)"
        tryOptionsRequest "https://fable.io"
        |> Promise.map (fun a ->
            match a with
            | Ok a -> "ohoh? Fable.io allows OPTIONS request?"
            | Error e -> failMessage)
        |> Promise.map (fun results ->
            results |> equal failMessage)

    it "fetchAs: works with manual decoder" <| fun () ->
        fetchAs "https://randomuser.me/api" RandomUser.ApiResult.Decoder []
        |> Promise.map (fun result ->
            equal result.Info.Page 1
        )

    it "fetchAs: fails if the decoder fail" <| fun () ->
        fetchAs "https://randomuser.me/api" RandomUser.UserName.Decoder []
        |> Promise.map (fun user ->
            user.First
        )
        |> Promise.catch (fun error ->
            error.Message
        )
        |> Promise.map(fun res ->
            res.StartsWith("Error at: `$.title`\nExpecting an object with a field named `title` but instead got:")
            |> equal true
        )

    it "fetchAs: works with auto decoder" <| fun () ->
        let apiResultDecoder = Decode.Auto.generateDecoder<RandomUser.ApiResult>(isCamelCase = true)
        fetchAs "https://randomuser.me/api" apiResultDecoder []
        |> Promise.map (fun result ->
            equal result.Info.Page 1
        )

    it "fetchAs: fails if the auto decoder fail" <| fun () ->
        let userNameDecoder = Decode.Auto.generateDecoder<RandomUser.UserName>(isCamelCase = true)
        fetchAs "https://randomuser.me/api" userNameDecoder []
        |> Promise.map (fun user ->
            user.First
        )
        |> Promise.catch (fun error ->
            error.Message
        )
        |> Promise.map(fun res ->
            res.StartsWith("Error at: `$.last`\nExpecting an object with a field named `last` but instead got:")
            |> equal true
        )

    it "tryFetchAs: works with manual decoder" <| fun () ->
        tryFetchAs "https://randomuser.me/api" RandomUser.ApiResult.Decoder []
        |> Promise.map (fun result ->
            match result with
            | Ok result ->
                equal result.Info.Page 1
            | Error _ ->
                failwith "This test should succeed"
        )

    it "tryFetchAs: fails if the decoder fail" <| fun () ->
        tryFetchAs "https://randomuser.me/api" RandomUser.UserName.Decoder []
        |> Promise.map (fun result ->
            match result with
            | Ok _ ->
                failwith "This test should fail"
            | Error msg -> msg
        )
        |> Promise.map(fun res ->
            res.StartsWith("Error at: `$.title`\nExpecting an object with a field named `title` but instead got:")
            |> equal true
        )

    it "tryFetchAs: works with auto decoder" <| fun () ->
        let apiResultDecoder = Decode.Auto.generateDecoder<RandomUser.ApiResult>(isCamelCase = true)
        tryFetchAs "https://randomuser.me/api" apiResultDecoder []
        |> Promise.map (fun result ->
            match result with
            | Ok result ->
                equal result.Info.Page 1
            | Error _ ->
                failwith "This test should succeed"
        )

    it "tryFetchAs: fails if the auto decoder fail" <| fun () ->
        let userNameDecoder = Decode.Auto.generateDecoder<RandomUser.UserName>(isCamelCase = true)
        tryFetchAs "https://randomuser.me/api" userNameDecoder []
        |> Promise.map (fun result ->
            match result with
            | Ok _ ->
                failwith "This test should fail"
            | Error msg -> msg
        )
        |> Promise.catch (fun error ->
            error.Message
        )
        |> Promise.map(fun res ->
            res.StartsWith("Error at: `$.last`\nExpecting an object with a field named `last` but instead got:")
            |> equal true
        )
