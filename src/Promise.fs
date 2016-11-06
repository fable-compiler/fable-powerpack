namespace Fable.PowerPack

#nowarn "1182" // Unused values

[<RequireQualifiedAccess>]
module Promise =
    open System
    open Fable.Core
    open Fable.Import
    open Fable.Core.JsInterop

    let inline private (!) (x:obj): 'T = unbox x

    [<Emit("new Promise($0)")>]
    /// The promise function receives two other function parameters: success and fail
    let create (f: ('T->unit)->(Exception->unit)->unit): JS.Promise<'T> = jsNative

    [<Emit("new Promise(resolve => setTimeout(resolve, $0))")>]
    let sleep (ms: int): JS.Promise<unit> = jsNative

    [<Emit("Promise.resolve($0)")>]
    let lift<'T> (a: 'T): JS.Promise<'T> = jsNative

    [<Emit("$1.then($0)")>]
    let bind (a: 'T->JS.Promise<'R>) (pr: JS.Promise<'T>): JS.Promise<'R> = jsNative

    [<Emit("$1.then($0)")>]
    let map (a: 'T->'R) (pr: JS.Promise<'T>): JS.Promise<'R> = jsNative

    [<Emit("$1.then($0)")>]
    let iter (a: 'T->unit) (pr: JS.Promise<'T>): unit = jsNative

    [<Emit("$1.catch($0)")>]
    let catch (a: obj->'T) (pr: JS.Promise<'T>): JS.Promise<'T> = jsNative

    [<Emit("$2.then($0,$1)")>]
    let either (success: 'T->'R) (fail: obj->'R) (pr: JS.Promise<'T>): JS.Promise<'R> = jsNative

    [<Emit("Promise.all($0)")>]
    let Parallel (pr: seq<JS.Promise<'T>>): JS.Promise<'T[]> = jsNative

    type PromiseBuilder() =
        [<Emit("$1.then($2)")>]
        member x.Bind(p: JS.Promise<'T>, f: 'T->JS.Promise<'R>): JS.Promise<'R> = jsNative

        [<Emit("$1.then(() => $2)")>]
        member x.Combine(p1: JS.Promise<unit>, p2: JS.Promise<'T>): JS.Promise<'T> = jsNative

        member x.For(seq: seq<'T>, body: 'T->JS.Promise<unit>): JS.Promise<unit> =
            // (lift (), seq)
            // ||> Seq.fold (fun p a ->
            //     bind (fun () -> body a) p)
            let mutable p = lift ()
            for a in seq do
                p <- !p?``then``(fun () -> body a)
            p

        member x.While(guard, p): JS.Promise<unit> =
            if guard()
            then bind (fun () -> x.While(guard, p)) p
            else lift()

        [<Emit("Promise.resolve($1)")>]
        member x.Return(a: 'T): JS.Promise<'T> = jsNative

        [<Emit("$1")>]
        member x.ReturnFrom(p: JS.Promise<'T>) = jsNative

        [<Emit("Promise.resolve()")>]
        member x.Zero(): JS.Promise<unit> = jsNative

        member x.TryFinally(p: JS.Promise<'T>, compensation: unit->unit) =
            either (fun x -> compensation(); x) (fun er -> compensation(); raise !er) p

        [<Emit("$1.catch($2)")>]
        member x.TryWith(p: JS.Promise<'T>, catchHandler: Exception->JS.Promise<'T>): JS.Promise<'T> = jsNative

        member x.Delay(generator: unit->JS.Promise<'T>): JS.Promise<'T> =
            !createObj[
                "then" ==> fun f1 f2 ->
                    try generator()?``then``(f1,f2)
                    with er ->
                        if box f2 = null
                        then !JS.Promise.reject(er)
                        else
                            try !JS.Promise.resolve(f2(er))
                            with er -> !JS.Promise.reject(er)
                "catch" ==> fun f ->
                    try generator()?catch(f)
                    with er ->
                        try !JS.Promise.resolve(f(er))
                        with er -> !JS.Promise.reject(er)
            ]

        member x.Using<'T, 'R when 'T :> IDisposable>(resource: 'T, binder: 'T->JS.Promise<'R>): JS.Promise<'R> =
            x.TryFinally(binder(resource), fun () -> resource.Dispose())

[<AutoOpen>]
module PromiseImpl =
    let promise = Promise.PromiseBuilder()