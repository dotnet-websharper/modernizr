// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

namespace WebSharper.Modernizr.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.Html.Client
open WebSharper.Modernizr

[<AutoOpen>]
module SampleInternals =

    [<JavaScript>]
    let DisplaySupport (feature: string) (x: bool) = 
        feature + if x then " is supported by your browser" else " is not supported in your browser"
        |> fun x -> H3 [Text x]

[<Sealed>]
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
        ] :> _
