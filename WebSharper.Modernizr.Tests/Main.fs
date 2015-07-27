namespace WebSharper.Modernizr.Tests

open WebSharper.Html.Server
open WebSharper
open WebSharper.Sitelets

type Action =
    | Home
    | About

module Skin =
    open System.Web

    type Page =
        {
            Title : string
            Body : list<Content.HtmlElement>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("body", fun x -> x.Body)

    let WithTemplate title body =
        Content.WithTemplate MainTemplate
            {
                Title = title
                Body = body
            }

module Site =

    let ( => ) text url =
        A [HRef url] -< [Text text]

    let HomePage ctx =
        Skin.WithTemplate "HomePage"
            [
                Div [new Samples()]
            ]

    let Main =
        Sitelet.Sum [
            Sitelet.Content "/" Home HomePage
        ]

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Home; About]

[<assembly: Website(typeof<Website>)>]
do ()
