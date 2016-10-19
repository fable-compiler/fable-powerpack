[<Fable.Core.EntryModule(".")>]
module Fable.PowerPack.Main

#r "../node_modules/fable-core/Fable.Core.dll"

// Attention! File order matters in F#.
// Promise must come before Fetch because
// the latter depends on the former.
#load
    "Promise.fsx"
    "Fetch.fsx"
