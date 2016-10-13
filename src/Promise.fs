namespace Fable.PowerPack

type Promise<'T> = Fable.Import.JS.Promise<'T>

[<RequireQualifiedAccess>]
module Promise =
    open System
    open Fable.Core
    open Fable.Import
    open Fable.Core.JsInterop

    let inline private (!) (x:obj): 'T = unbox x

    [<Emit("new Promise($0)")>]
    /// The promise function receives two other function parameters: success and fail
    let create (f: ('T->unit)->(Exception->unit)->unit): Promise<'T> = jsNative

    [<Emit("new Promise(resolve => setTimeout(resolve, $0))")>]
    let sleep (ms: int): Promise<unit> = jsNative

    [<Emit("Promise.resolve($0)")>]
    let lift<'T> (a: 'T): Promise<'T> = jsNative

    [<Emit("$1.then($0)")>]
    let bind (a: 'T->Promise<'R>) (pr: Promise<'T>): Promise<'R> = jsNative

    [<Emit("$1.then($0)")>]
    let map (a: 'T->'R) (pr: Promise<'T>): Promise<'R> = jsNative

    [<Emit("$1.then($0)")>]
    let iter (a: 'T->unit) (pr: Promise<'T>): unit = jsNative

    [<Emit("$1.catch($0)")>]
    let catch (a: obj->'T) (pr: Promise<'T>): Promise<'T> = jsNative
        
    [<Emit("$2.catch($0,$1)")>]
    let either (success: 'T->'R) (fail: obj->'R) (pr: Promise<'T>): Promise<'R> = jsNative

    type PromiseBuilder() =
        [<Emit("$1.catch($2)")>]
        member x.Bind(p: Promise<'T>, f: 'T->Promise<'R>): Promise<'R> = jsNative
        [<Emit("$1.catch(() => $2)")>]
        member x.Combine(p1: Promise<unit>, p2: Promise<'T>): Promise<'T> = jsNative
        member x.For(seq: seq<'T>, body: 'T->Promise<unit>): Promise<unit> =
            let mutable p = lift ()
            for a in seq do
                p <- !p?``then``(fun () -> body a)
            p
        member x.While(guard, p): Promise<unit> =
            if guard()
            then bind (fun () -> x.While(guard, p)) p
            else lift()
        [<Emit("Promise.resolve($1)")>]
        member x.Return(a: 'T): Promise<'T> = jsNative
        [<Emit("$1")>]
        member x.ReturnFrom(p: Promise<'T>) = jsNative
        [<Emit("Promise.resolve()")>]
        member x.Zero(): Promise<unit> = jsNative
        member x.TryFinally(p: Promise<'T>, compensation: unit->unit) =
            either (fun x -> compensation(); x) (fun er -> compensation(); raise !er) p
        [<Emit("$1.catch($2)")>]
        member x.TryWith(p: Promise<'T>, catchHandler: Exception->Promise<'T>): Promise<'T> = jsNative
        member x.Delay(generator: unit->Promise<'T>): Promise<'T> =
            !(createObj [
                "then" ==> fun f1 f2 ->
                    try generator()?``then``(f1,f2)
                    with er -> f2(er)
                "catch" ==> fun f ->
                    try generator()?catch(f)
                    with er -> f(er)
            ])
        member x.Using<'T, 'R when 'T :> IDisposable>(resource: 'T, binder: 'T->Promise<'R>): Promise<'R> =
            x.TryFinally(binder(resource), fun () -> resource.Dispose())

[<AutoOpen>]
module PromiseImpl =
    let promise = Promise.PromiseBuilder()