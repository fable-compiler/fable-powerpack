[<RequireQualifiedAccess>]
module Components.Navbar

    open Fable.Helpers.React
    open Fable.Helpers.React.Props
    open Fulma
    open Fulma.FontAwesome

    let private shadow =
        div [ ClassName "bd-special-shadow"
              Style [ Opacity 1.
                      Transform "scaleY(1)" ] ]
            [ ]

    let private viewSimpleIcon isHiddenDesktop =
        let inline props key url = Navbar.Item.Props [ Key ("simple-icon-"+ key)
                                                       Href url ]

        let visibility =
            if isHiddenDesktop then
                "is-hidden-desktop"
            else
                "is-hidden-mobile"

        [ Navbar.Item.a [ props "Github" "https://github.com/fable-compiler/fable-powerpack"
                          Navbar.Item.CustomClass visibility ]
            [ Icon.faIcon [ ]
                [ Fa.icon Fa.I.Github
                  Fa.faLg ] ]
          Navbar.Item.a [ props "Twitter" "https://twitter.com/FableCompiler"
                          Navbar.Item.CustomClass ("twitter " + visibility) ]
            [ Icon.faIcon [ ]
                [ Fa.icon Fa.I.Twitter
                  Fa.faLg ] ]
        ] |> ofList

    let tweetUrl = "https://twitter.com/FableCompiler"

    let private viewButton =
        Navbar.Item.div [ ]
            [ Field.div [ Field.IsGrouped ]
                [ Control.p [ ]
                    [ Button.a [ Button.CustomClass "twitter"
                                 Button.Props [ Href tweetUrl
                                                Target "_blank" ] ]
                        [ Icon.faIcon [ ]
                            [ Fa.icon Fa.I.Twitter
                              Fa.faLg ]
                          span [ ] [ str "Tweet" ]
                        ]
                    ]
                  Control.p [ ]
                    [ Button.a [ Button.CustomClass "github"
                                 Button.Props [ Href "https://github.com/fable-compiler/fable-powerpack" ] ]
                        [ Icon.faIcon [ ]
                            [ Fa.icon Fa.I.Github
                              Fa.faLg ]
                          span [ ] [ str "Github" ]
                        ]
                    ]
                ]
            ]

    let private navbarEnd =
        Navbar.End.div [ ]
            [ viewSimpleIcon false
              viewButton ]

    let private directItem name pageUrl currentUrl =
        Navbar.Item.a [ Navbar.Item.Props [ Href pageUrl ]
                        Navbar.Item.IsActive (pageUrl = currentUrl ) ]
                [ str name ]

    let private navbarMenu pageUrl =
        Navbar.menu [ Navbar.Menu.Props [ Id "navMenu" ] ]
            [ directItem "Date format" Route.DateFormat pageUrl
              directItem "Fetch" Route.Fetch pageUrl
              directItem "Json" Route.Json pageUrl
              directItem "Promises" Route.Promise pageUrl ]

    let private navbarBrand =
        Navbar.Brand.div [ ]
            [ Navbar.Item.a [ Navbar.Item.Props [ Href Route.Index ] ]
                [ Heading.p [ Heading.Is4 ]
                    [ str "Fable.PowerPack" ] ]
              viewSimpleIcon true
              Navbar.burger [ ]
                [ span [ ] [ ]
                  span [ ] [ ]
                  span [ ] [ ] ] ]

    let render pageUrl =
        Navbar.navbar [ Navbar.Color (IsCustomColor "thoth-primary")
                        Navbar.CustomClass "is-fixed-top" ]
            [ shadow
              Container.container [ ]
                [ navbarBrand
                  navbarMenu pageUrl
                  navbarEnd ] ]
