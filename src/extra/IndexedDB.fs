module Fable.PowerPack.Experimental.IndexedDB

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.PowerPack

type DBKeyMethod<'T,'TKey> =
    | KeyPath of string
    | AutoIncrement

type DBCursorDirection =
    /// This direction causes the cursor to be opened at the start of the source.
    /// When iterated, the cursor should yield all records, including duplicates, in monotonically increasing order of keys.
    | Next
    /// This direction causes the cursor to be opened at the start of the source.
    /// When iterated, the cursor should not yield records with the same key, but otherwise yield all records, in monotonically increasing order of keys.
    /// For every key with duplicate values, only the first record is yielded.
    /// When the source is an object store or a unique index, this direction has the exact same behavior as Next.
    | NextUnique
    /// This direction causes the cursor to be opened at the end of the source.
    /// When iterated, the cursor should yield all records, including duplicates, in monotonically decreasing order of keys.
    | Prev
    /// This direction causes the cursor to be opened at the end of the source.
    /// When iterated, the cursor should not yield records with the same key, but otherwise yield all records, in monotonically decreasing order of keys.
    /// For every key with duplicate values, only the first record is yielded.
    /// When the source is an object store or a unique index, this direction has the exact same behavior as Prev.
    | PrevUnique
    static member Default with get() = Next
    override dir.ToString() =
        match dir with
        | Next -> "next"
        | NextUnique -> "nextunique"
        | Prev -> "prev"
        | PrevUnique -> "prevunique"

let inline createStore<'T,'TKey>(keyMethod: DBKeyMethod<'T,'TKey>) (db: Browser.IDBDatabase) =
    let args = createEmpty<Browser.IDBObjectStoreParameters>
    match keyMethod with
    | KeyPath path -> args.keyPath <- Some(U2.Case1 path)
    | AutoIncrement -> args.autoIncrement <- Some true
    db.createObjectStore(typeof<'T>.Name, args)

let inline deleteStore<'T>(db: Browser.IDBDatabase) =
    db.deleteObjectStore(typeof<'T>.Name)

type IDBImplementation =
    abstract member Version: uint32
    abstract member Upgrade: Browser.IDBDatabase -> unit

let openCursor(index: Browser.IDBIndex, keyCursor: bool, range: Browser.IDBKeyRange option, direction: DBCursorDirection option, step: uint32 option) =
    let direction = (defaultArg direction DBCursorDirection.Default).ToString()
    let step = defaultArg step 1u
    let request =
        match keyCursor with
        | false ->
          match range with
          | Some range -> index.openCursor(range, direction=direction)
          | None -> index.openCursor(?range=None, direction=direction)
        | true ->
          match range with
          | Some range -> index.openKeyCursor(range, direction=direction)
          | None -> index.openKeyCursor(?range=None, direction=direction)
    let rec cursorSeq() = promiseSeq {
        let! result = Promise.create(fun cont _ ->
            request.onsuccess <- fun _ ->
                request.onsuccess <- !!null
                if unbox<bool> request.result then
                    let cursor = unbox<Browser.IDBCursorWithValue> request.result
                    cont(Some(unbox<'T> cursor.value))
                    cursor.advance(float step)
                else
                    cont(None)
                null)
        match result with
        | None -> ()
        | Some r ->
            yield r
            yield! cursorSeq()
    }
    cursorSeq()

let countAllAsync(x: Browser.IDBIndex) =
    Promise.create(fun cont econt ->
        let request = x.count()
        request.onerror <- fun _ -> box(econt(exn request.error.name))
        request.onsuccess <- fun _ -> box(cont(unbox<int> request.result))
    )

/// The parameter should not target the object primary key, but the property used by the index.
let countKeyAsync(indexKey: 'TIndex) (x: Browser.IDBIndex) =
    Promise.create(fun cont econt ->
        let request = x.count(indexKey)
        request.onerror <- fun _ -> box(econt(exn request.error.name))
        request.onsuccess <- fun _ -> box(cont(unbox<int> request.result))
    )

/// The parameter should not target the object primary key, but the property used by the index.
let countRangeAsync(indexKeyRange: Browser.IDBKeyRange) (x: Browser.IDBIndex) =
    Promise.create(fun cont econt ->
        let request = x.count(indexKeyRange)
        request.onerror <- fun _ -> box(econt(exn request.error.name))
        request.onsuccess <- fun _ -> box(cont(unbox<int> request.result))
    )

/// The parameter should not target the object primary key, but the property used by the index.
let getAsync(indexKey: 'TIndex) (x: Browser.IDBIndex) =
    Promise.create(fun cont econt ->
        let request = x.get(indexKey)
        request.onerror <- fun _ -> box(econt(exn request.error.name))
        request.onsuccess <- fun _ -> box(cont(unbox<'T> request.result))
    )

/// The parameter should not target the object primary key, but the property used by the index.
let getFirstAsync(indexKeyRange: Browser.IDBKeyRange) (x: Browser.IDBIndex) =
    Promise.create(fun cont econt ->
        let request = x.get(indexKeyRange)
        request.onerror <- fun _ -> box(econt(exn request.error.name))
        request.onsuccess <- fun _ -> box(cont(unbox<'T> request.result))
    )

/// The parameter should not target the object primary key, but the property used by the index.
/// The return value is the object actual primary key.
let getKeyAsync(indexKey: obj) (x: Browser.IDBIndex) =
    Promise.create(fun cont econt ->
        let request = x.getKey(indexKey)
        request.onerror <- fun _ -> box(econt(exn request.error.name))
        request.onsuccess <- fun _ -> box(cont(request.result))
    )

/// The parameter should not target the object primary key, but the property used by the index.
/// The return value is the object actual primary key.
let getKeyFirstAsync(indexKeyRange: Browser.IDBKeyRange) (x: Browser.IDBIndex) =
    Promise.create(fun cont econt ->
        let request = x.getKey(indexKeyRange)
        request.onerror <- fun _ -> box(econt(exn request.error.name))
        request.onsuccess <- fun _ -> box(cont(request.result))
    )

// /// If no range is passed, then the range includes all records.
// /// If no direction is passed, it will default to "next".
// member x.openCursor(?range: Browser.IDBKeyRange, ?direction: DBCursorDirection, ?step: uint32) =
//     DBSeqCursor<'T>.openCursor(unbox x, keyCursor=false, range=range, direction=direction, step=step)

// /// If no range is passed, then the range includes all keys.
// /// If no direction is passed, it will default to "next".
// member x.openKeyCursor(?range: Browser.IDBKeyRange, ?direction: DBCursorDirection, ?step: uint32) =
//     DBSeqCursor<obj>.openCursor(unbox x, keyCursor=true, range=range, direction=direction, step=step)

type Browser.IDBObjectStore with
    member x.getAsync<'T>(key: obj) =
        Promise.create(fun cont econt ->
            let request = x.get(key)
            request.onerror <- fun _ -> box(econt(exn request.error.name))
            request.onsuccess <- fun _ -> box(cont(unbox<'T> request.result))
        )

    /// If no range is passed, it will default to a key range that selects all the records in this object store.
    /// If no direction is passed, it will default to "next".
    member x.openCursorAsync(?range: Browser.IDBKeyRange, ?direction: DBCursorDirection, ?step: uint32) =
        openCursor(unbox x, false, range, direction, step)

    // /// Insert only method. The operation is asynchronous but request is being ignored.
    // member x.add(item: 'T) =
    //     ignore(x.add(item))

    /// Insert only method. It can be used in promise workflows. The return value is key set for the stored record.
    /// Note the continuation is called when the operation is added to the queue, but you still need to wait for the transaction to complete.
    member x.addAsync(item: 'T) =
        Promise.create(fun cont econt ->
            let request = x.add(item)
            request.onerror <- fun _ -> box(econt(exn request.error.name))
            request.onsuccess <- fun _ -> box(cont(request.result))
        )

    // /// Delete the item specified by the key. The operation is asynchronous but request is being ignored.
    // member x.delete(key: obj) =
    //     ignore(x.delete(key))

    /// Delete the item specified by the key. It can be used in promise workflows.
    /// Note the continuation is called when the operation is added to the queue, but you still need to wait for the transaction to complete.
    member x.deleteAsync(key: obj) =
        Promise.create(fun cont econt ->
            let request = x.delete(key)
            request.onerror <- fun _ -> box(econt(exn request.error.name))
            request.onsuccess <- fun _ -> box(cont())
        )

    // /// Delete all items in the store. The operation is asynchronous but request is being ignored.
    // member x.clear() =
    //     ignore(x.clear())

    /// Delete all items in the store. It can be used in promise workflows.
    /// Note the continuation is called when the operation is added to the queue, but you still need to wait for the transaction to complete.
    member x.clearAsync() =
        Promise.create(fun cont econt ->
            let request = x.clear()
            request.onerror <- fun _ -> box(econt(exn request.error.name))
            request.onsuccess <- fun _ -> box(cont())
        )

    // /// Update or Insert method. The operation is asynchronous but request is being ignored.
    // member x.put(item: 'T) =
    //     ignore(x.put(item))

    /// Update or insert method. It can be used in promise workflows. The return value is key set for the stored record.
    /// Note the continuation is called when the operation is added to the queue, but you still need to wait for the transaction to complete.
    member x.putAsync(item: 'T) =
        Promise.create(fun cont econt ->
            let request = x.put(item)
            request.onerror <- fun _ -> box(econt(exn request.error.name))
            request.onsuccess <- fun _ -> box(cont(request.result))
        )

type IndexedDB<'T when 'T :> IDBImplementation and 'T : (new: unit->'T)>() =
    member inline x.DeleteDatabase() =
        Promise.create(fun cont econt ->
            let name = typeof<'T>.Name
            let request = Browser.indexedDB.deleteDatabase(name)
            request.onerror <- fun _ -> box(econt(exn request.error.name))
            request.onsuccess <- fun _ -> box(cont())
        )

    member inline private x.Use(mkTransaction: Browser.IDBDatabase->Browser.IDBTransaction) (execTransaction: Browser.IDBTransaction->JS.Promise<'Result>) =
        Promise.create(fun cont econt ->
            let impl = new 'T() :> IDBImplementation
            let name = typeof<'T>.Name
            let request = Browser.indexedDB.``open``(name, float impl.Version)
            request.onerror <- fun _ ->
                econt(exn request.error.name)
                null
            request.onupgradeneeded <- fun ev ->
                try
                    let db = unbox<Browser.IDBDatabase> request.result
                    impl.Upgrade(db)
                with
                    | e -> econt(e)
                null
            request.onsuccess <- fun _ ->
                try
                    let db = unbox<Browser.IDBDatabase> request.result
                    let trans = mkTransaction db
                    execTransaction trans
                    |> Promise.map (fun res ->
                        trans.oncomplete <- fun _ -> db.close(); box(cont(res))
                        trans.onerror <- fun _ -> db.close(); box(econt(exn trans.error.name)))
                    |> Promise.catch econt
                    |> ignore
                with
                | e -> econt(e)
                null
        )

    member inline x.UseStore<'S1,'Result>(transaction: Browser.IDBObjectStore->JS.Promise<'Result>) =
        let storeName1 = typeof<'S1>.Name
        let mkTransaction = fun (db: Browser.IDBDatabase) ->
            db.transaction(U2.Case1 storeName1, "readonly")
        let execTransaction = fun (trans: Browser.IDBTransaction) ->
            let store1 = trans.objectStore(storeName1)
            transaction store1
        x.Use mkTransaction execTransaction

    member inline x.UseStore<'S1,'S2,'Result>(transaction: Browser.IDBObjectStore->Browser.IDBObjectStore->JS.Promise<'Result>) =
        let storeName1 = typeof<'S1>.Name
        let storeName2 = typeof<'S2>.Name
        let mkTransaction = fun (db: Browser.IDBDatabase) ->
            db.transaction(U2.Case2(ResizeArray[|storeName1;storeName2|]), "readonly")
        let execTransaction = fun (trans: Browser.IDBTransaction) ->
            let store1 = trans.objectStore(storeName1)
            let store2 = trans.objectStore(storeName2)
            transaction store1 store2
        x.Use mkTransaction execTransaction

    member inline x.UseStore<'S1,'S2,'S3,'Result>(transaction: Browser.IDBObjectStore->Browser.IDBObjectStore->Browser.IDBObjectStore->JS.Promise<'Result>) =
        let storeName1 = typeof<'S1>.Name
        let storeName2 = typeof<'S2>.Name
        let storeName3 = typeof<'S3>.Name
        let mkTransaction = fun (db: Browser.IDBDatabase) ->
            db.transaction(U2.Case2(ResizeArray[|storeName1;storeName2;storeName3|]), "readonly")
        let execTransaction = fun (trans: Browser.IDBTransaction) ->
            let store1 = trans.objectStore(storeName1)
            let store2 = trans.objectStore(storeName2)
            let store3 = trans.objectStore(storeName3)
            transaction store1 store2 store3
        x.Use mkTransaction execTransaction

    member inline x.UseStore<'S1,'S2,'S3,'S4,'Result>(transaction: Browser.IDBObjectStore->Browser.IDBObjectStore->Browser.IDBObjectStore->Browser.IDBObjectStore->JS.Promise<'Result>) =
        let storeName1 = typeof<'S1>.Name
        let storeName2 = typeof<'S2>.Name
        let storeName3 = typeof<'S3>.Name
        let storeName4 = typeof<'S4>.Name
        let mkTransaction = fun (db: Browser.IDBDatabase) ->
            db.transaction(U2.Case2(ResizeArray[|storeName1;storeName2;storeName3;storeName4|]), "readonly")
        let execTransaction = fun (trans: Browser.IDBTransaction) ->
            let store1 = trans.objectStore(storeName1)
            let store2 = trans.objectStore(storeName2)
            let store3 = trans.objectStore(storeName3)
            let store4 = trans.objectStore(storeName4)
            transaction store1 store2 store3 store4
        x.Use mkTransaction execTransaction

    member inline x.UseStoreRW<'S1,'Result>(transaction: Browser.IDBObjectStore->JS.Promise<'Result>) =
        let storeName1 = typeof<'S1>.Name
        let mkTransaction = fun (db: Browser.IDBDatabase) ->
            db.transaction(U2.Case1 storeName1, "readwrite")
        let execTransaction = fun (trans: Browser.IDBTransaction) ->
            let store1 = trans.objectStore(storeName1)
            transaction store1
        x.Use mkTransaction execTransaction

    member inline x.UseStoreRW<'S1,'S2,'Result>(transaction: Browser.IDBObjectStore->Browser.IDBObjectStore->JS.Promise<'Result>) =
        let storeName1 = typeof<'S1>.Name
        let storeName2 = typeof<'S2>.Name
        let mkTransaction = fun (db: Browser.IDBDatabase) ->
            db.transaction(U2.Case2(ResizeArray[|storeName1; storeName2|]), "readwrite")
        let execTransaction = fun (trans: Browser.IDBTransaction) ->
            let store1 = trans.objectStore(storeName1)
            let store2 = trans.objectStore(storeName2)
            transaction store1 store2
        x.Use mkTransaction execTransaction

    member inline x.UseStoreRW<'S1,'S2,'S3,'Result>(transaction: Browser.IDBObjectStore->Browser.IDBObjectStore->Browser.IDBObjectStore->JS.Promise<'Result>) =
        let storeName1 = typeof<'S1>.Name
        let storeName2 = typeof<'S2>.Name
        let storeName3 = typeof<'S3>.Name
        let mkTransaction = fun (db: Browser.IDBDatabase) ->
            db.transaction(U2.Case2(ResizeArray[|storeName1;storeName2;storeName3|]), "readwrite")
        let execTransaction = fun (trans: Browser.IDBTransaction) ->
            let store1 = trans.objectStore(storeName1)
            let store2 = trans.objectStore(storeName2)
            let store3 = trans.objectStore(storeName3)
            transaction store1 store2 store3
        x.Use mkTransaction execTransaction

    member inline x.UseStoreRW<'S1,'S2,'S3,'S4,'Result>(transaction: Browser.IDBObjectStore->Browser.IDBObjectStore->Browser.IDBObjectStore->Browser.IDBObjectStore->JS.Promise<'Result>) =
        let storeName1 = typeof<'S1>.Name
        let storeName2 = typeof<'S2>.Name
        let storeName3 = typeof<'S3>.Name
        let storeName4 = typeof<'S4>.Name
        let mkTransaction = fun (db: Browser.IDBDatabase) ->
            db.transaction(U2.Case2(ResizeArray[|storeName1;storeName2;storeName3;storeName4|]), "readwrite")
        let execTransaction = fun (trans: Browser.IDBTransaction) ->
            let store1 = trans.objectStore(storeName1)
            let store2 = trans.objectStore(storeName2)
            let store3 = trans.objectStore(storeName3)
            let store4 = trans.objectStore(storeName4)
            transaction store1 store2 store3 store4
        x.Use mkTransaction execTransaction