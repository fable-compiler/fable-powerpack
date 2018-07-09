[<RequireQualifiedAccess>]
module Route

#if DEBUG
let [<Literal>] Host = "/"
#else
let [<Literal>] Host = "http://fable.io/fable-powerpack/"
#endif

let [<Literal>] Index = Host + "index.html"
let [<Literal>] DateFormat = Host + "date-format.html"
let [<Literal>] Fetch = Host + "fetch.html"
let [<Literal>] Json = Host + "json.html"
let [<Literal>] Promise = Host + "promise.html"
