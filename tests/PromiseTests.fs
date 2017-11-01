module PromiseTests

open System
open Fable.PowerPack
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import

let inline equal (expected: 'T) (actual: 'T): unit =
    let assert' = importAll<obj> "assert"
    assert'?equal(actual, expected) |> ignore

[<Global>]
let it (msg: string) (f: unit->JS.Promise<'T>): unit = jsNative

[<Global("it")>]
let itSync (msg: string) (f: unit->unit): unit = jsNative

[<Global>]
let describe (msg: string) (f: unit->unit): unit = jsNative

type DisposableAction(f) =
    interface IDisposable with
        member __.Dispose() = f()

describe "Promise tests" <| fun _ ->
    it "Simple promise translates without exception" <| fun () ->
        promise { return () }

    it "PromiseBuilder.Combine works" <| fun () ->
        let nums = [|1;2;3;4;5|]
        promise {
            let mutable xs = []
            for x in nums do
                let x = x + 1
                if x < 5 then
                    xs <- x::xs
            return xs
        }
        |> Promise.map (fun xs -> xs = [4;3;2] |> equal true)

    it "Promise for binding works correctly" <| fun () ->
        let inputs = [|1; 2; 3|]
        let result = ref 0
        promise {
            for inp in inputs do
                result := !result + inp
        }
        |> Promise.map (fun () -> equal !result 6)

    it "Promise while binding works correctly" <| fun () ->
        let mutable result = 0
        promise {
            while result < 10 do
                result <- result + 1
        }
        |> Promise.map (fun () -> equal result 10)

    it "Promise exceptions are handled correctly" <| fun () ->
        let result = ref 0
        let f shouldThrow =
            promise {
                try
                    if shouldThrow then failwith "boom!"
                    else result := 12
                with _ -> result := 10
            } |> Promise.map (fun () -> !result)
        promise {
            let! x = f true
            let! y = f false
            return x + y
        } |> Promise.map (equal 22)

    it "Simple promise is executed correctly" <| fun () ->
        let result = ref false
        let x = promise { return 99 }
        promise {
            let! x = x
            let y = 99
            result := x = y
        }
        |> Promise.map (fun () -> equal !result true)

    it "promise use statements should dispose of resources when they go out of scope" <| fun () ->
        let isDisposed = ref false
        let step1ok = ref false
        let step2ok = ref false
        let resource = promise {
            return new DisposableAction(fun () -> isDisposed := true)
        }
        promise {
            use! r = resource
            step1ok := not !isDisposed
        }
        |> Promise.map (fun () ->
            step2ok := !isDisposed
            (!step1ok && !step2ok) |> equal true)

    it "Try ... with ... expressions inside promise expressions work the same" <| fun () ->
        let result = ref ""
        let throw() : unit =
            raise(exn "Boo!")
        let append(x) =
            result := !result + x
        let innerPromise() =
            promise {
                append "b"
                try append "c"
                    throw()
                    append "1"
                with _ -> append "d"
                append "e"
            }
        promise {
            append "a"
            try do! innerPromise()
            with _ -> append "2"
            append "f"
        } |> Promise.map (fun () ->
            equal !result "abcdef")

    it "Promise try .. with returns correctly from 'with' branch" <| fun () ->
        let work = promise {
            try
              failwith "testing"
              return -1
            with e ->
              return 42 }
        work |> Promise.map (equal 42)

    // it "Deep recursion with promise doesn't cause stack overflow" <| fun () ->
    //     promise {
    //         let result = ref false
    //         let rec trampolineTest res i = promise {
    //             if i > 100000
    //             then res := true
    //             else return! trampolineTest res (i+1)
    //         }
    //         do! trampolineTest result 0
    //         equal !result true
    //     }

    it "Nested failure propagates in promise expressions" <| fun () ->
        promise {
            let data = ref ""
            let f1 x =
                promise {
                    try
                        failwith "1"
                        return x
                    with
                    | e -> return! failwith ("2 " + e.Message.Trim('"'))
                }
            let f2 x =
                promise {
                    try
                        return! f1 x
                    with
                    | e -> return! failwith ("3 " + e.Message.Trim('"'))
                }
            let f() =
                promise {
                    try
                        let! y = f2 4
                        return ()
                    with
                    | e -> data := e.Message.Trim('"')
                }
            do! f()
            do! Promise.sleep 100
            equal "3 2 1" !data
        }

    it "Try .. finally expressions inside promise expressions work" <| fun () ->
        promise {
            let data = ref ""
            do! promise {
                try data := !data + "1 "
                finally data := !data + "2 "
            }
            do! promise {
                try
                    try failwith "boom!"
                    finally data := !data + "3"
                with _ -> ()
            }
            do! Promise.sleep 100
            equal "1 2 3" !data
        }

    it "Final statement inside promise expressions can throw" <| fun () ->
        promise {
            let data = ref ""
            let f() = promise {
                try data := !data + "1 "
                finally failwith "boom!"
            }
            do! promise {
                try
                    do! f()
                    return ()
                with
                | e -> data := !data + e.Message.Trim('"')
            }
            do! Promise.sleep 100
            equal "1 boom!" !data
        }

    it "Promise.Bind propagates exceptions" <| fun () ->
        promise {
            let task2 name = promise {
                // printfn "testing with %s" name
                do! Promise.sleep 100 //difference between task1 and task2
                if name = "fail" then
                    failwith "Invalid access credentials"
                return "Ok"
            }

            let doWork name task =
                promise {
                    let! b =
                        task "fail"
                        |> Promise.catch (fun ex -> ex.Message)
                    return b
                }

            let! res2 = doWork "task2" task2
            equal "Invalid access credentials" res2
        }

    it "Promise.catchBind takes a Promise-returning function" <| fun () ->
        promise {
            let pr = promise {
                failwith "Boo!"
                return "Ok"
            }
            let exHandler (e: exn) = promise {
                return e.Message
            }

            let! res = pr |> Promise.catchBind exHandler
            res |> equal "Boo!"
        }

    it "Promise.either can take all combinations of value-returning and Promise-returning continuations" <| fun () ->
        promise {
            let failing = promise { failwith "Boo!" }
            let successful = Promise.lift 42

            let! r1 = successful |> Promise.either (fun x -> !^(string x)) (fun x -> failwith "Shouldn't get called")
            let! r2 = successful |> Promise.either (fun x -> !^(Promise.lift <| string x)) (fun x -> failwith "Shouldn't get called")
            let! r3 = failing |> Promise.either (fun x -> failwith "Shouldn't get called") (fun (ex:Exception) -> !^ex.Message)
            let! r4 = failing |> Promise.either (fun x -> failwith "Shouldn't get called") (fun (ex:Exception) -> !^(Promise.lift ex.Message))

            r1 |> equal "42"
            r2 |> equal "42"
            r3 |> equal "Boo!"
            r4 |> equal "Boo!"
        }

    itSync "Promise.start works" <| fun () ->
        promise {
            // Whitespaces are just for a better display in the console
            printfn "    Promise started"
            return 5
        } |> Promise.start
