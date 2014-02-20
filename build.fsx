#load "tools/includes.fsx"
open IntelliFactory.Core
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.Modernizr", "2.5-alpha").Configure(fun bt ->
        bt
        |> Logs.Config.Custom (Logs.Default.Verbose().ToConsole()))
    |> fun bt -> bt.WithFramework(bt.Framework.Net40)

let main =
    bt.WebSharper
        .Extension("IntelliFactory.WebSharper.Modernizr")
        .SourcesFromProject()
        .Embed(["modernizr-1.6.min.js"])


let tests =
    bt.WebSharper.Library("IntelliFactory.WebSharper.Modernizr.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project(main)
                r.Assembly("System.Web")
            ])

bt.Solution [

    main
    tests

    bt.WebSharper.HostWebsite("Website")
        .References(fun r ->
            [
                r.Project tests
                r.Project main
            ])

    bt.NuGet.CreatePackage()
        .ProjectUrl("http://bitbucket.org/intellifactory/websharper.modernizr")
        .Description("WebSharper bindings to the Modernizr library 1.6.")
        .Add(main)

]
|> bt.Dispatch
