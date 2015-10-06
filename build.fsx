#load "tools/includes.fsx"
open IntelliFactory.Core
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.Modernizr")
        .VersionFrom("WebSharper")
        .WithFSharpVersion(FSharpVersion.FSharp30)
        .WithFramework(fun fw -> fw.Net40)

let main =
    bt.WebSharper
        .Extension("WebSharper.Modernizr")
        .SourcesFromProject()
        .Embed(["modernizr-1.6.min.js"])


let tests =
    bt.WebSharper.Library("WebSharper.Modernizr.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project(main)
                r.Assembly("System.Web")
                r.NuGet("WebSharper.Html").Reference()
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
