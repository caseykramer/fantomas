﻿module Fantomas.Tests.FormattingSelectionTests

open NUnit.Framework
open FsUnit

open Fantomas.CodeFormatter
open Fantomas.Tests.TestHelper

[<Test>]
let ``should format a part of a line correctly``() =
    formatSelectionFromString false (makeRange 3 8 3 11) """
let x = 2 + 3
let y = 1+2
let z = x + y""" config
    |> should equal """
let x = 2 + 3
let y = 1 + 2
let z = x + y"""

[<Test>]
let ``should format a whole line correctly and preserve indentation``() =
    formatSelectionFromString false (makeRange 3 4 3 34) """
    let base1 = d1 :> Base1
    let derived1 = base1 :?> Derived1""" config
    |> should equal """
    let base1 = d1 :> Base1
    let derived1 = base1 :?> Derived1"""

[<Test>]
let ``should format a few lines correctly and preserve indentation``() =
    formatSelectionFromString false (makeRange 3 5 5 51) """
let rangeTest testValue mid size =
    match testValue with
    | var1 when var1 >= mid - size/2 && var1 <= mid + size/2 -> printfn "The test value is in range."
    | _ -> printfn "The test value is out of range."

let (var1, var2) as tuple1 = (1, 2)""" config
    |> should equal """
let rangeTest testValue mid size =
    match testValue with
    | var1 when var1 >= mid - size / 2 && var1 <= mid + size / 2 -> 
        printfn "The test value is in range."
    | _ -> printfn "The test value is out of range."

let (var1, var2) as tuple1 = (1, 2)"""

[<Test>]
let ``should format a top-level let correctly``() =
    formatSelectionFromString false (makeRange 3 0 3 11) """
let x = 2 + 3
let y = 1+2
let z = x + y""" config
    |> should equal """
let x = 2 + 3
let y = 1 + 2
let z = x + y"""

[<Test>]
let ``should ignore whitespace at the beginning of lines``() =
    formatSelectionFromString false (makeRange 3 3 3 27) """
type Product' (backlogItemId) =
    let mutable ordering = 0
    let mutable version = 0
    let backlogItems = []""" config
    |> should equal """
type Product' (backlogItemId) =
    let mutable ordering = 0
    let mutable version = 0
    let backlogItems = []"""

[<Test>]
let ``should parse a complete expression correctly``() =
    formatSelectionFromString false (makeRange 4 0 5 35) """
open Fantomas.CodeFormatter

let config = { FormatConfig.Default with 
                IndentSpaceNum = 2 }

let source = "
    let Multiple9x9 () = 
      for i in 1 .. 9 do
        printf \"\\n\";
        for j in 1 .. 9 do
          let k = i * j in
          printf \"%d x %d = %2d \" i j k;
          done;
      done;;
    Multiple9x9 ();;"
"""     config
    |> should equal """
open Fantomas.CodeFormatter

let config = { FormatConfig.Default with IndentSpaceNum = 2 }

let source = "
    let Multiple9x9 () = 
      for i in 1 .. 9 do
        printf \"\\n\";
        for j in 1 .. 9 do
          let k = i * j in
          printf \"%d x %d = %2d \" i j k;
          done;
      done;;
    Multiple9x9 ();;"
"""

[<Test>]
let ``should format the selected pipeline correctly``() =
    formatSelectionFromString false (makeRange 3 4 7 16) """
let r = 
    [ "abc"
      "a"
      "b"
      "" ]
    |> List.map id""" config
    |> should equal """
let r = 
    ["abc"
     "a"
     "b"
     ""]
    |> List.map id
"""