module DateFormatTests

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.PowerPack

let inline equal (expected: 'T) (actual: 'T): unit =
    let assert' = importAll<obj> "assert"
    assert'?deepStrictEqual(actual, expected) |> ignore

[<Global>]
let it (msg: string) (f: unit->unit): unit = jsNative

[<Global>]
let describe (msg: string) (f: unit->unit): unit = jsNative

describe "DateFormat tests" <| fun _ ->

    describe "Pattern 'd': The day of the month, from 1 through 31" <| fun () ->
        it "1 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            Date.Format.format testDate "d"
            |> equal "8"

        it "2 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 0)
            Date.Format.format testDate "d"
            |> equal "22"

    describe "Pattern 'dd': The day of the month, from 01 through 31" <| fun () ->
        it "1 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            Date.Format.format testDate "dd"
            |> equal "08"

        it "2 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 0)
            Date.Format.format testDate "dd"
            |> equal "22"

    describe "Pattern 'ddd': The abbreviated name of the day of the week" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            Date.Format.format testDate "ddd"
            |> equal "Tue"

    describe "Pattern 'dddd': The full name of the day of the week" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            Date.Format.format testDate "dddd"
            |> equal "Tuesday"

    describe "Pattern 'h': The hour, using a 12-hour clock from 1 to 12" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            Date.Format.format testDate "h"
            |> equal "1"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            Date.Format.format testDate "h"
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            Date.Format.format testDate "h"
            |> equal "3"

    describe "Pattern 'hh': The hour, using a 12-hour clock from 01 to 12" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            Date.Format.format testDate "hh"
            |> equal "01"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            Date.Format.format testDate "hh"
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            Date.Format.format testDate "hh"
            |> equal "03"

    describe "Pattern 'H': The hour, using a 24-hour clock from 0 to 23" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            Date.Format.format testDate "H"
            |> equal "1"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            Date.Format.format testDate "H"
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            Date.Format.format testDate "H"
            |> equal "15"

    describe "Pattern 'HH': The hour, using a 24-hour clock from 00 to 23" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            Date.Format.format testDate "HH"
            |> equal "01"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            Date.Format.format testDate "HH"
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            Date.Format.format testDate "HH"
            |> equal "15"

    describe "Pattern 'm': The minute, from 0 through 59" <| fun () ->
        it "1 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format testDate "m"
            |> equal "4"

        it "2 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 22, 0)
            Date.Format.format testDate "m"
            |> equal "22"

    describe "Pattern 'mm': The minute, from 00 through 59" <| fun () ->
        it "1 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format testDate "mm"
            |> equal "04"

        it "2 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 22, 0)
            Date.Format.format testDate "mm"
            |> equal "22"

    describe "Pattern 'M': The month, from 1 through 12" <| fun () ->
        it "1 digit month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format testDate "M"
            |> equal "8"

        it "2 digit month works" <| fun () ->
            let testDate = DateTime(2017, 11, 22, 0, 22, 0)
            Date.Format.format testDate "M"
            |> equal "11"

    describe "Pattern 'MM': The month, from 01 through 12" <| fun () ->
        it "1 digit month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format testDate "MM"
            |> equal "08"

        it "2 digit month works" <| fun () ->
            let testDate = DateTime(2017, 11, 22, 0, 22, 0)
            Date.Format.format testDate "MM"
            |> equal "11"

    describe "Pattern 'MMM': The abbreviated name of the month" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format testDate "MMM"
            |> equal "Aug"

    describe "Pattern 'MMMM': The full name of the month" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format testDate "MMMM"
            |> equal "August"

    describe "Pattern 's': The second, from 00 through 59" <| fun () ->
        it "1 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 3)
            Date.Format.format testDate "s"
            |> equal "3"

        it "2 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 33)
            Date.Format.format testDate "s"
            |> equal "33"

    describe "Pattern 'ss': The second, from 00 through 59" <| fun () ->
        it "1 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 3)
            Date.Format.format testDate "ss"
            |> equal "03"

        it "2 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 33)
            Date.Format.format testDate "ss"
            |> equal "33"

    describe "Pattern 't': The first character of the AM/PM designator" <| fun () ->
        it "AM designators with 2 or more letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 3)
            Date.Format.format testDate "t"
            |> equal "A"

        it "PM designators with 2 or more letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 3)
            Date.Format.format testDate "t"
            |> equal "P"

        it "AM designators with less then 2 letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.localFormat Date.Local.french testDate "t"
            |> equal ""

        it "PM designators with less then 2 letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 15, 0, 33)
            Date.Format.localFormat Date.Local.french testDate "t"
            |> equal ""

    describe "Pattern 'tt': The AM/PM designator" <| fun () ->
        it "AM designators" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 3)
            Date.Format.format testDate "tt"
            |> equal "AM"

        it "PM designators" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 3)
            Date.Format.format testDate "tt"
            |> equal "PM"

        it "Empty AM designators works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.localFormat Date.Local.french testDate "tt"
            |> equal ""

        it "Empty PM designators works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 15, 0, 33)
            Date.Format.localFormat Date.Local.french testDate "tt"
            |> equal ""

    describe "Pattern 'y': The year, from 0 to 99" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format testDate "y"
            |> equal "1"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format testDate "y"
            |> equal "0"

        it "4 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(1900, 1, 1, 0, 0, 0)
            Date.Format.format testDate "y"
            |> equal "0"

        it "4 digit year ending with non zero (0-9) works" <| fun () ->
            let testDate = DateTime(2009, 1, 1, 0, 0, 0)
            Date.Format.format testDate "y"
            |> equal "9"

        it "4 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format testDate "y"
            |> equal "19"

    describe "Pattern 'yy': The year, from 00 to 99" <| fun () ->
        it "1 digit year ending with non zero (0-9) works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format testDate "yy"
            |> equal "01"

        it "2 digit year ending with 0 works" <| fun () ->
            let testDate = DateTime.Parse("0010-01-01")
            Date.Format.format testDate "yy"
            |> equal "10"

        it "2 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format testDate "yy"
            |> equal "01"

        it "2 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime.Parse("0011-01-01")
            Date.Format.format testDate "yy"
            |> equal "11"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yy"
            |> equal "00"

        it "3 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime(905, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yy"
            |> equal "05"

        it "3 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(911, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yy"
            |> equal "11"

        it "4 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(1900, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yy"
            |> equal "00"

        it "4 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime(2009, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yy"
            |> equal "09"

        it "4 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yy"
            |> equal "19"

    describe "Pattern 'yyy': The year, with a minimum of three digits" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format testDate "yyy"
            |> equal "001"

        it "2 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0011-01-01")
            Date.Format.format testDate "yyy"
            |> equal "011"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yyy"
            |> equal "900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yyy"
            |> equal "2019"

    describe "Pattern 'yyyy': The year as a four-digit number" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format testDate "yyyy"
            |> equal "0001"

        it "2 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0011-01-01")
            Date.Format.format testDate "yyyy"
            |> equal "0011"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yyyy"
            |> equal "0900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yyyy"
            |> equal "2019"

    describe "Pattern 'yyyyy': The year as a five-digit number" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format testDate "yyyyy"
            |> equal "00001"

        it "2 digit year works" <| fun () ->
            let testDate = DateTime.Parse("0099-01-01")
            Date.Format.format testDate "yyyyy"
            |> equal "00099"

        it "3 digit year works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yyyyy"
            |> equal "00900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format testDate "yyyyy"
            |> equal "02019"

    describe "Escaping character work" <| fun () ->
        it "Escape work" <| fun _ ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.format testDate "yyyy-MM-dd hh:mm:ss \d \y \M\M"
            |> equal "2017-08-22 01:00:33 d y MM"

    describe "Localization" <| fun _ ->
        it "French works" <| fun _ ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.localFormat Date.Local.french testDate "dddd"
            |> equal "Mardi"

        it "English works" <| fun _ ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.localFormat Date.Local.english testDate "dddd"
            |> equal "Tuesday"
