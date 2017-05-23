namespace Fable.PowerPack

#nowarn "1182" // Unused values

[<RequireQualifiedAccess>]
module EventSouce =
    open System
    open Fable.Core

    type Event =
        abstract data: string with get

    type EventSource =
        abstract onmessage: (Event -> unit) with get, set

    [<Emit("new EventSource($0)")>]
    /// The promise function receives two other function parameters: success and fail
    let create (location: string): EventSource = jsNative

