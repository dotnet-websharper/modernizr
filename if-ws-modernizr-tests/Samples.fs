// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2010.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.Modernizr.Tests

// open IntelliFactory.WebSharper
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Modernizr

module SampleInternals =
    [<JavaScript>]
    let DisplaySupport (feature: string) (x: bool) = 
        feature + if x then " is supported by your browser" else " is not supported in your browser"
        |> fun x -> H3 [Text x]
        
open SampleInternals
type Samples() = 
    inherit Web.Control()

    [<JavaScript>]
    override this.Body = 
        Div [
            H1 [Text "Modernizr"]
            DisplaySupport "Audio" Modernizr.Audio
            DisplaySupport "Canvas" Modernizr.Canvas
            DisplaySupport "Canvas Text" Modernizr.CanvasText
            DisplaySupport "Drag-and-Drop" Modernizr.DragAndDrop
            DisplaySupport "Geolocation" Modernizr.Geolocation
            DisplaySupport "History" Modernizr.History
            DisplaySupport "Video" Modernizr.Video
            DisplaySupport "Webgl" Modernizr.Webgl
        ] :> IPagelet
        


