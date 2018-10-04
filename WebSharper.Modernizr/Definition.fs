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

namespace WebSharper.Modernizr

open System
open System.IO
open System.Collections.Generic
open WebSharper.InterfaceGenerator

module Renamer =

    let Words =
        Path.Combine(__SOURCE_DIRECTORY__, "words.txt")
        |> File.ReadAllLines
        |> Set.ofArray

    let SplitAt (s:string) (n:int) =
        if n < s.Length && n > 0 then
            (s.Substring(0, n), s.Substring(n))
        else (s, "")

    let Parts (s:string) =
        Seq.map (SplitAt s) {1..(s.Length-1)}
    
    let Capitalize (s:string) = 
        s.Substring(0, 1).ToUpper() + s.Substring(1)

    let rec ToPascalCase (w: string) (n: int) =
        if n <= 1 && Words.Contains w then Capitalize w |> Seq.singleton
        elif n <= 1 then Seq.empty
        else seq { for (f, l) in Parts w do
                       for c in ToPascalCase f 1 do
                           yield! Seq.map ((+) c) (ToPascalCase l (n-1))
                 }

    let Rename (w: string) (n:int) =
        match Seq.toList <| ToPascalCase w n with
        | [w] -> w
        | [] -> failwithf "no possible renaming of %s" w
        | r -> failwith ("Ambiguous renaming: " + r.ToString())

module Definition =

    let inline RenamedGetter (s: string) (t) (n: int) =
        s =? t |> WithSourceName (Renamer.Rename s n)

    let Availability =
        let Availability = Class "Availability"
        Availability
        |+> Static [
            "maybe" =? Availability 
            "probably" =? Availability
            "notAvailable" =? Availability |> WithGetterInline "''"
        ]

    let AudioFormat = 
        let AudioFormat = Class "AudioFormat"
        AudioFormat
        |+> Instance [
            "ogg" =? Availability
            "mp3" =? Availability
            "wav" =? Availability
            "m4a" =? Availability
        ]
    
    let VideoFormat = 
        let VideoFormat = Class "VideoFormat"
        VideoFormat
        |+> Instance [
            "ogg" =? Availability
            "h264" =? Availability
        ]

    let InputType =
        let InputType = Class "InputType"
        InputType
        |+> Instance [
            "search" =? T<bool>
            "tel" =? T<bool>
            "url" =? T<bool>
            "email" =? T<bool>
            "datetime" =? T<bool> |> WithSourceName "DateTime"
            "date" =? T<bool>
            "month" =? T<bool>
            "week" =? T<bool>
            "time" =? T<bool>
            "datetime-local" =? T<bool> |> WithSourceName "DateTimeLocal"
            "number" =? T<bool>
            "range" =? T<bool>
            "color" =? T<bool>
        ]

    let Input =
        let Input = Class "Input"
        Input
        |+> Instance [
            "autocomplete" =? T<bool>
            "autofocus" =? T<bool>
            "list" =? T<bool>
            "placeholder" =? T<bool>
            "max" =? T<bool>
            "min" =? T<bool>
            "multiple" =? T<bool>
            "pattern" =? T<bool>
            "required" =? T<bool>
            "step" =? T<bool>
        ]

    let Modernizr =
        Class "Modernizr"
        |=> Nested [
            Availability
            AudioFormat
            VideoFormat
            Input
            InputType
        ]
        |+> Static [
            "fontface" =? T<bool>
            "canvas" =? T<bool>
            RenamedGetter "canvastext" T<bool> 2
            "audio" =? T<bool>
            "video" =? T<bool>
            "rgba" =? T<bool>
            "hsla" =? T<bool>
            RenamedGetter "borderimage" T<bool> 2
            RenamedGetter "borderradius" T<bool> 2
            RenamedGetter "boxshadow" T<bool> 2
            "multiplebgs" =? T<bool>
            RenamedGetter "backgroundsize" T<bool> 2
            "opacity" =? T<bool>
            "cssanimations" =? T<bool>
            "csscolumns" =? T<bool>
            "cssgradients" =? T<bool>
            "cssreflections" =? T<bool>
            "csstransforms" =? T<bool>
            "csstransforms3d" =? T<bool>
            "csstransitions" =? T<bool>
            "geolocation" =? T<bool> 
            RenamedGetter "localstorage" T<bool> 2
            RenamedGetter "sessionstorage" T<bool> 2
            RenamedGetter "webworkers" T<bool> 2
            RenamedGetter "applicationcache" T<bool> 2
            "svg" =? T<bool>
            "inlinesvg" =? T<bool>
            "svgclippaths" =? T<bool>
            "smil" =? T<bool>
            "websqldatabase" =? T<bool>
            "indexeddb" =? T<bool>
            RenamedGetter "websockets" T<bool> 2
            RenamedGetter "hashchange" T<bool> 2
            "history" =? T<bool>
            RenamedGetter "draganddrop" T<bool> 3
            RenamedGetter "postmessage" T<bool> 2
            RenamedGetter "audioformat" AudioFormat 2
            RenamedGetter "videoformat" VideoFormat 2
            RenamedGetter "inputtypes" InputType 2
            RenamedGetter "textshadow" T<bool> 2
            "touch" =? T<bool>
            "webgl" =? T<bool>
            "input" =? Input
            "flexbox" =? T<bool>
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.Modernizr" [
                 Modernizr
            ]
            Namespace "WebSharper.Modernizr.Resources" [
                let r = Resource "Modernizr" "modernizr-1.6.min.js"
                yield r.AssemblyWide() :> _
            ]
        ]

open WebSharper.InterfaceGenerator

[<Sealed>]
type ModernizrExtension() =
    interface IExtension with
        member ext.Assembly = Definition.Assembly

[<assembly: Extension(typeof<ModernizrExtension>)>]
do ()
