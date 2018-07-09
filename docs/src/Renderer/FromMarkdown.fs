[<RequireQualifiedAccess>]
module Renderer.FromMarkdown

    open Helpers

    let render page filePath title =
        let filePath = "${entryDir}/src/Content/" + filePath
        Page.render {
            PageUrl = page
            Title = title
            Body = parseMarkdown (resolve filePath)
        }
