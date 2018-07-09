module Main

open Renderer

FromMarkdown.render (Route.Index) "index.md" "Home"
FromMarkdown.render (Route.DateFormat) "date_format.md" "Date format"
FromMarkdown.render (Route.Fetch) "fetch.md" "Fetch"
FromMarkdown.render (Route.Json) "json.md" "Json"
FromMarkdown.render (Route.Promise) "promises.md" "Promises"
