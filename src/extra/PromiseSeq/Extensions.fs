// ----------------------------------------------------------------------------
// F# promise extensions (PromiseSeq.fs)
// (c) Tomas Petricek, 2011, Available under Apache 2.0 license.
// ----------------------------------------------------------------------------
namespace Fable.PowerPack

open Fable.Import

[<AutoOpen>]
module PromiseSeqExtensions =

  /// Builds an asynchronou sequence using the computation builder syntax
  let promiseSeq = new PromiseSeq.PromiseSeqBuilder()

  // Add asynchronous for loop to the 'promise' computation builder
  type Promise.PromiseBuilder with
    member x.For (seq:PromiseSeq<'T>, action:'T -> JS.Promise<unit>) =
      promise.Bind(seq, function
        | Nil -> promise.Zero()
        | Cons(h, t) -> promise.Combine(action h, x.For(t, action)))