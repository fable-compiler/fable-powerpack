#r "../_bin/Fornax.Core.dll"
#load "../siteModel.fsx"
#load "default.fsx"

open Html
open SiteModel

type Model = {
    title : string
}

let generate (siteModel : SiteModel) (mdl : Model) (posts : Post list) (content : string) =
    let pageContent =
        [ h1 [ Id "post-title" ] [!! mdl.title]
          !! content ]

    Default.defaultPage siteModel mdl.title posts pageContent