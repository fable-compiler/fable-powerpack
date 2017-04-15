module Fable.PowerPack.Json

open Fable.Core

type Json =
    | String of string
    | Number of float
    | Object of array<string * Json>
    | Array of array<Json>
    | Boolean of bool
    | Null

// TODO implement this in F# rather than the FFI
[<Import("ofString", "./Json.js")>]
let private _ofString:
    System.Func<string -> Json,
                float -> Json,
                array<string * Json> -> Json,
                array<Json> -> Json,
                bool -> Json,
                Json,
                Json -> Result<Json, System.Exception>,
                System.Exception -> Result<Json, System.Exception>,
                string -> Result<Json, System.Exception>> = jsNative

let ofString: string -> Result<Json, System.Exception> = _ofString.Invoke(String, Number, Object, Array, Boolean, Null, Ok, Error)
