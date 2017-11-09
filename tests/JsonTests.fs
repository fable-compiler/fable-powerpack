module JsonTests

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.PowerPack.Json

let inline equal (expected: 'T) (actual: 'T): unit =
    Testing.Assert.AreEqual(expected, actual)

[<Global>]
let it (msg: string) (f: unit->unit): unit = jsNative

[<Global>]
let describe (msg: string) (f: unit->unit): unit = jsNative

let isError = function Error _ -> true | Ok _ -> false

describe "Json tests" <| fun _ ->
    it "Parsing works" <| fun () ->
        ofString "{}" |> equal (Ok (Object [||]))

    it "Parsing strings works" <| fun () ->
        ofString "\"foo\"" |> equal (Ok (String "foo"))

    it "Parsing integers works" <| fun () ->
        ofString "50" |> equal (Ok (Number 50.0))

    it "Parsing floats works" <| fun () ->
        ofString "50.5" |> equal (Ok (Number 50.5))

    it "Parsing booleans works" <| fun () ->
        ofString "true" |> equal (Ok (Boolean true))
        ofString "false" |> equal (Ok (Boolean false))

    it "Parsing null works" <| fun () ->
        ofString "null" |> equal (Ok Null)

    it "Parsing objects works" <| fun () ->
        ofString "{ \"foo\": null, \"bar\": 1.0, \"qux\": {} }" |> equal (Ok (Object [| ("foo", Null); ("bar", Number 1.0); ("qux", Object [||]) |]))

    it "Parsing arrays works" <| fun () ->
        ofString "[1, 2, 3]" |> equal (Ok (Array [| Number 1.0; Number 2.0; Number 3.0 |]))

    it "Parsing bad input works I" <| fun () ->
        // TODO put the syntax error message
        ofString "" |> isError |> equal true

    it "Parsing bad input works II" <| fun () ->
        // TODO put the syntax error message
        ofString "{" |> isError |> equal true
