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

namespace IntelliFactory.WebSharper.Utils
open System.Collections.Generic

module Renamer =
    
    let Words = 
        let set = new HashSet<string>()
        for w in System.IO.File.ReadAllLines "../../words.txt" do
            set.Add w |> ignore
        set

    let SplitAt (s:string) (n:int) =
        if n < s.Length && n > 0 then
            (s.Substring(0, n), s.Substring(n))
        else (s, "")

    let Parts (s:string) =
        Seq.map (SplitAt s) {1..(s.Length-1)}
    
    let Capitalize (s:string) = 
        s.Substring(0, 1).ToUpper() + s.Substring(1)
            
    let rec ToPascalCase (w: string) (n: int) =
        if   n <= 1 && Words.Contains w then Capitalize w |> Seq.singleton
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
        




