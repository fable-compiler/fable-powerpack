module Fable.PowerPack.Result

let unwrapResult a =
    match a with
    | Ok a -> a
    | Error b -> raise b

// TODO implement TryFinally, TryWith, Using, While, and For ?
type ResultBuilder() =
    member this.Bind(m, f) = Result.bind f m
    member this.Return<'A, 'E>(a: 'A): Result<'A, 'E> = Ok a
    member this.ReturnFrom(m) = m
    member this.Zero() = this.Return()

    member this.Combine<'A, 'E>(left: Result<unit, 'E>, right: Result<'A, 'E>): Result<'A, 'E> =
        this.Bind(left, fun () -> right)

    (*member this.For<'A, 'E>(s: seq<'A>, fn: ('A -> Result<unit, 'E>)): Result<unit, 'E> =
        let error = Seq.tryFind (fun a ->
            match fn a with
            | Ok () -> false
            | Error _ -> true) s

        match error with
        | Some e -> e
        | None -> this.Zero()*)

let result = ResultBuilder()
