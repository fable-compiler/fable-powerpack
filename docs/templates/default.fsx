#r "../_bin/Fornax.Core.dll"
#load "../siteModel.fsx"

open Html
open SiteModel

// Toggle this lines when testing locally
// let host = "/"
let host = "http://fable.io/fable-powerpack/"

type NavbarButton =
    { ButtonClass : string
      IconClass : string
      Text : string }

let javascript =
    """
// Init highlight
hljs.initHighlightingOnLoad();

// Set active menu
var pageTitle = document.getElementById("post-title");

if (pageTitle) {
    var suffix = pageTitle.innerText.toLowerCase();
    var activeTab = document.getElementById("tab-" + suffix);
    activeTab.classList.add("is-active");
} else
    document.getElementById("tab-home").classList.add("is-active");
    """

let navbarButton (info: NavbarButton) =
     p [ Class "control" ]
       [ a  [ Class (info.ButtonClass + " button")
              Href ""
              Target "_blank" ]
            [ span [ Class "icon" ]
                   [ i [ Class ("fa " + info.IconClass) ] [ ] ]
              span [ ] [ !!info.Text ] ] ]

let defaultPage (siteModel : SiteModel) pageTitle (posts: Post list) content =
    let postsTabs =
        posts
        |> List.map (fun p ->
            li [ Id ("tab-" + p.title.ToLower()) ]
               [ a [ Href (host + (p.link.Substring(1))) ] [ !!p.title ] ]
        )

    let homeTab =
        li [ Id "tab-home" ]
           [ a [ Href host ] [ !!"Home" ] ]

    html [] [
        head [] [
            meta [CharSet "utf-8"]
            title [] [ !!("Fable Powerpack | " + pageTitle) ]
            link [ Rel "shortcut icon"; Href (host + "assets/favicon-16x16.png")]
            // Icons and global style
            link [ Rel "stylesheet"; Type "text/css"; Href "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" ]
            link [ Rel "stylesheet"; Type "text/css"; Href "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.5.1/css/bulma.min.css" ]
            // Highlight.js
            link [ Rel "stylesheet"; Type "text/css"; Href "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.12.0/styles/atom-one-dark.min.css" ]
            script [ Src "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.12.0/highlight.min.js" ] [ ]
            script [ Src "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.12.0/languages/fsharp.min.js" ] [ ]
            link [ Rel "stylesheet"; Type "text/css"; Href (host + "css/style.css") ]
        ]
        body []
            [ section [ Class "hero is-fable-blue" ]
                      [ div [ Class "hero-body" ]
                            [ div [ Class "container" ]
                                  [ div [ Class "columns is-vcentered" ]
                                        [ div [ Class "column is-4 is-offset-4 has-text-centered" ]
                                              [ p [ Class "title" ]
                                                  [ !!"Fable Powerpack" ]
                                                p [ Class "subtitle" ]
                                                  [ !!"Utilities for your Fable apps" ] ] ] ] ]
                        div [ Class "hero-footer" ]
                            [ div [ Class "container" ]
                                  [ div [ Class "tabs is-centered is-toggle" ]
                                        [ ul [ ]
                                             [ yield homeTab
                                               yield! postsTabs ] ] ] ] ]
              section [ Class "section" ]
                      [ div [ Class "container" ]
                            [ div [ Class "content" ] content ] ]

              script [ ]
                     [ !!javascript ]
            ]
        ]
