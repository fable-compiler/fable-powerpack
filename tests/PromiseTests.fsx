#r "../node_modules/fable-core/Fable.Core.dll"
#r "../npm/Fable.PowerPack.dll"

open System
open Fable.PowerPack
open Fable.Core.Testing

let equal (expected: 'T) (actual: 'T) =
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Simple promise translates without exception``() =
    promise { return () }
    |> ignore

[<Test>]
let ``Promise for binding works correctly``() =
    let inputs = [|1; 2; 3|]
    let result = ref 0
    promise { 
        for inp in inputs do
            result := !result + inp
    }
    |> Promise.iter (fun () -> equal !result 6)

[<Test>]
let ``Promise while binding works correctly``() =
    let mutable result = 0
    promise { 
        while result < 10 do
            result <- result + 1
    }
    |> Promise.iter (fun () -> equal result 10)

[<Test>]
let ``Promise exceptions are handled correctly``() =
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
    } |> Promise.iter (equal 22)  

[<Test>]
let ``Simple promise is executed correctly``() =
    let result = ref false
    let x = promise { return 99 }
    promise { 
        let! x = x
        let y = 99
        result := x = y
    }
    |> Promise.iter (fun () -> equal !result true)

type DisposableAction(f) =
    interface IDisposable with
        member __.Dispose() = f()

[<Test>]
let ``promise use statements should dispose of resources when they go out of scope``() =
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
    |> Promise.iter (fun () ->
        step2ok := !isDisposed
        (!step1ok && !step2ok) |> equal true)

[<Test>]
let ``Try ... with ... expressions inside promise expressions work the same``() =
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
    } |> Promise.iter (fun () ->
        equal !result "abcdef")

[<Test>]
let ``Promise try .. with returns correctly from 'with' branch``() =
    let work = promise { 
        try 
          failwith "testing"
          return -1
        with e ->
          return 42 }
    work |> Promise.iter (equal 42)

// [<Test>]
// let ``Deep recursion with promise doesn't cause stack overflow``() =
//     promise {
//         let result = ref false
//         let rec trampolineTest res i = promise {
//             if i > 100000
//             then res := true
//             else return! trampolineTest res (i+1)
//         }
//         do! trampolineTest result 0
//         equal !result true
//     } |> Promise.iter ignore

[<Test>]
let ``Nested failure propagates in promise expressions``() =
    promise {
        let data = ref ""
        let f1 x = 
            promise {
                try
                    failwith "1"
                    return x
                with
                | e -> return! failwith ("2 " + e.Message) 
            }
        let f2 x = 
            promise {
                try
                    return! f1 x
                with
                | e -> return! failwith ("3 " + e.Message) 
            }
        let f() =
            promise { 
                try
                    let! y = f2 4
                    return ()
                with
                | e -> data := e.Message
            }
        do! f()
        do! Promise.sleep 100
        equal "3 2 1" !data
    } |> Promise.iter ignore

[<Test>]
let ``Try .. finally expressions inside promise expressions work``() =
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
    } |> Promise.iter ignore

[<Test>]
let ``Final statement inside promise expressions can throw``() =
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
            | e -> data := !data + e.Message
        }
        do! Promise.sleep 100
        equal "1 boom!" !data
    } |> Promise.iter ignore
