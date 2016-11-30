// ----------------------------------------------------------------------------
// F# promise extensions (PromiseSeq.fs)
// (c) Tomas Petricek, 2011, Available under Apache 2.0 license.
// ----------------------------------------------------------------------------
namespace Fable.PowerPack

open Fable.Import

/// An asynchronous sequence represents a delayed computation that can be
/// started to produce either Cons value consisting of the next element of the
/// sequence (head) together with the next asynchronous sequence (tail) or a
/// special value representing the end of the sequence (Nil)
type PromiseSeq<'T> = JS.Promise<PromiseSeqInner<'T>>

/// The interanl type that represents a value returned as a result of
/// evaluating a step of an asynchronous sequence
and PromiseSeqInner<'T> =
  | Nil
  | Cons of 'T * PromiseSeq<'T>
