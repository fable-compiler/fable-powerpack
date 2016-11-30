// Adapted from the example in MDN: mdn.github.io/IDBcursor-example

#r "../../node_modules/fable-core/Fable.Core.dll"
#r "../../npm/Fable.PowerPack.dll"

open Fable.Core
open Fable.Import
open Fable.Import.Browser
open Fable.PowerPack
open Fable.PowerPack.Experimental.IndexedDB

let fire pr: obj =
    Promise.iter ignore pr; null

// Define the type we'll use to create the Object Store
// The store will have the name of the type
type Record =
    {
        albumTitle: string
        year: int
    }
    static member CreateDummyData() = [|
      { albumTitle= "Power windows"; year= 1985 }
      { albumTitle= "Grace under pressure"; year= 1984 }
      { albumTitle= "Signals"; year= 1982 }
      { albumTitle= "Moving pictures"; year= 1981 }
      { albumTitle= "Permanent waves"; year= 1980 }
      { albumTitle= "Hemispheres"; year= 1978 }
      { albumTitle= "A farewell to kings"; year= 1977 }
      { albumTitle= "2112"; year= 1976 }
      { albumTitle= "Caress of steel"; year= 1975 }
      { albumTitle= "Fly by night"; year= 1975 }
      { albumTitle= "Rush"; year= 1974 }
    |]

// Define a type with default constructor and implementing the DBImplementation interface
// The name of this type will be used to identify the database
// When we open the database, if it doesn't exists or it does with an inferior version number,
// Upgrade will be triggered
type RecordDB() =
    interface IDBImplementation with
        member x.Version with get() = 1u
        member x.Upgrade(db) =
            let store = createStore<Record,string> (KeyPath "albumTitle") db
            store.createIndex("year", U2.Case1 "year") |> ignore

let displayData(cursorGetter: IDBObjectStore->PromiseSeq<Record>) = promise {
    try
        let listEl: HTMLElement = unbox(document.querySelector "ul")
        listEl.innerHTML <- ""
        let db = IndexedDB<RecordDB>()
        do! db.UseStore<Record,_>(fun cs -> promise {
            let cursor = cursorGetter(cs)
            for record in cursor do
                let item = document.createElement("li")
                item.innerHTML <- "<strong>" + record.albumTitle + "</strong>, " + (string record.year)
                listEl.appendChild item |> ignore
        })
      with
      | ex -> console.error("Error when displaying data: %O", ex)
    }

let populatedata(records: #seq<Record>) = promise {
    try
        let db = IndexedDB<RecordDB>()
        do! db.UseStoreRW<Record,_>(fun cs -> promise { for r in records do cs.put(r) })
        do! displayData(fun cs -> cs.openCursorAsync())
    with
    | ex -> console.error("Error when populating data: ", ex)
}

let main() =
    // Add events
    unbox<HTMLElement>(document.querySelector ".continue").onclick <- fun _ ->
        displayData(fun cs -> cs.openCursorAsync()) |> fire

    unbox<HTMLElement>(document.querySelector ".advance").onclick <- fun _ ->
        displayData(fun cs -> cs.openCursorAsync(step=2u)) |> fire

    unbox<HTMLElement>(document.querySelector ".direction").onclick <- fun _ ->
        displayData(fun cs -> cs.openCursorAsync(direction=DBCursorDirection.Prev)) |> fire

    unbox<HTMLElement>(document.querySelector ".delete").onclick <- fun _ ->
        promise {
            let db = IndexedDB<RecordDB>()
            do! db.UseStoreRW<Record,_>(fun cs -> promise {
                do! cs.deleteAsync("Grace under pressure")
                console.log("Deleted that mediocre album from 1984. Even Power windows is better.")
            })
            do! displayData(fun cs -> cs.openCursorAsync())
        } |> fire

    unbox<HTMLElement>(document.querySelector ".update").onclick <- fun _ ->
        promise {
            let db = IndexedDB<RecordDB>()
            do! db.UseStoreRW<Record,_>(fun cs -> promise {
                let! key = cs.putAsync({ albumTitle="A farewell to kings"; year=2050 })
                console.log("A better album year?") })
            do! displayData(fun cs -> cs.openCursorAsync())
        } |> fire

    // Populate the database with initial data
    Record.CreateDummyData()
    |> populatedata
    |> fire

main()
