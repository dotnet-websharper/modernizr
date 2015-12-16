#load "tools/includes.fsx"
open IntelliFactory.Core
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("Zafir.Modernizr")
        .VersionFrom("Zafir")
        .WithFSharpVersion(FSharpVersion.FSharp30)
        .WithFramework(fun fw -> fw.Net40)

let main =
    bt.Zafir
        .Extension("WebSharper.Modernizr")
        .SourcesFromProject()
        .Embed(["modernizr-1.6.min.js"])


let tests =
    bt.Zafir.Library("WebSharper.Modernizr.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project(main)
                r.Assembly("System.Web")
                r.NuGet("Zafir.Html").Reference()
            ])

bt.Solution [

    main
    tests

    bt.Zafir.HostWebsite("Website")
        .References(fun r ->
            [
                r.Project tests
                r.Project main
            ])

    bt.NuGet.CreatePackage()
        .ProjectUrl("http://bitbucket.org/intellifactory/websharper.modernizr")
        .Description("Zafir bindings to the Modernizr library 1.6.")
        .Add(main)

]
|> bt.Dispatch
