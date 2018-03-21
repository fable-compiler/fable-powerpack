module DateFormatTests

open System
open Fable.Core
open Fable.PowerPack

let inline equal (expected: 'T) (actual: 'T): unit =
    Testing.Assert.AreEqual(expected, actual)

[<Global>]
let it (msg: string) (f: unit->unit): unit = jsNative

[<Global>]
let describe (msg: string) (f: unit->unit): unit = jsNative

describe "DateFormat tests" <| fun _ ->
    let localeEnglish = Date.Local.english
    let localeFrench = Date.Local.french
    let localeRussian = Date.Local.russian
    let localeHungarian = Date.Local.hungarian

    describe "Pattern 'd': The day of the month, from 1 through 31" <| fun () ->
        it "1 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            Date.Format.format "d" testDate
            |> equal "8"

        it "2 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 0)
            Date.Format.format "d" testDate
            |> equal "22"

    describe "Pattern 'dd': The day of the month, from 01 through 31" <| fun () ->
        it "1 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            Date.Format.format "dd" testDate
            |> equal "08"

        it "2 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 0)
            Date.Format.format "dd" testDate
            |> equal "22"

    describe "Pattern 'ddd': The abbreviated name of the day of the week" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            Date.Format.format "ddd" testDate
            |> equal "Tue"

    describe "Pattern 'dddd': The full name of the day of the week" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            Date.Format.format "dddd" testDate
            |> equal "Tuesday"

    describe "Pattern 'h': The hour, using a 12-hour clock from 1 to 12" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            Date.Format.format "h" testDate
            |> equal "1"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            Date.Format.format "h" testDate
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            Date.Format.format "h" testDate
            |> equal "3"

    describe "Pattern 'hh': The hour, using a 12-hour clock from 01 to 12" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            Date.Format.format "hh" testDate
            |> equal "01"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            Date.Format.format "hh" testDate
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            Date.Format.format "hh" testDate
            |> equal "03"

    describe "Pattern 'H': The hour, using a 24-hour clock from 0 to 23" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            Date.Format.format "H" testDate
            |> equal "1"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            Date.Format.format "H" testDate
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            Date.Format.format "H" testDate
            |> equal "15"

    describe "Pattern 'HH': The hour, using a 24-hour clock from 00 to 23" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            Date.Format.format "HH" testDate
            |> equal "01"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            Date.Format.format "HH" testDate
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            Date.Format.format "HH" testDate
            |> equal "15"

    describe "Pattern 'm': The minute, from 0 through 59" <| fun () ->
        it "1 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format "m" testDate
            |> equal "4"

        it "2 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 22, 0)
            Date.Format.format "m" testDate
            |> equal "22"

    describe "Pattern 'mm': The minute, from 00 through 59" <| fun () ->
        it "1 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format "mm" testDate
            |> equal "04"

        it "2 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 22, 0)
            Date.Format.format "mm" testDate
            |> equal "22"

    describe "Pattern 'M': The month, from 1 through 12" <| fun () ->
        it "1 digit month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format "M" testDate
            |> equal "8"

        it "2 digit month works" <| fun () ->
            let testDate = DateTime(2017, 11, 22, 0, 22, 0)
            Date.Format.format "M" testDate
            |> equal "11"

    describe "Pattern 'MM': The month, from 01 through 12" <| fun () ->
        it "1 digit month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format "MM" testDate
            |> equal "08"

        it "2 digit month works" <| fun () ->
            let testDate = DateTime(2017, 11, 22, 0, 22, 0)
            Date.Format.format "MM" testDate
            |> equal "11"

    describe "Pattern 'MMM': The abbreviated name of the month" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format "MMM" testDate
            |> equal "Aug"

    describe "Pattern 'MMMM': The full name of the month" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            Date.Format.format "MMMM" testDate
            |> equal "August"

    describe "Pattern 's': The second, from 0 through 59" <| fun () ->
        it "1 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 3)
            Date.Format.format "s" testDate
            |> equal "3"

        it "2 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 33)
            Date.Format.format "s" testDate
            |> equal "33"

    describe "Pattern 'ss': The second, from 00 through 59" <| fun () ->
        it "1 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 3)
            Date.Format.format "ss" testDate
            |> equal "03"

        it "2 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 33)
            Date.Format.format "ss" testDate
            |> equal "33"

    describe "Pattern 't': The first character of the AM/PM designator" <| fun () ->
        it "AM designators with 2 or more letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 3)
            Date.Format.format "t" testDate
            |> equal "A"

        it "PM designators with 2 or more letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 3)
            Date.Format.format "t" testDate
            |> equal "P"

        it "AM designators with less then 2 letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.localFormat localeFrench "t" testDate
            |> equal ""

        it "PM designators with less then 2 letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 15, 0, 33)
            Date.Format.localFormat localeFrench "t" testDate
            |> equal ""

    describe "Pattern 'tt': The AM/PM designator" <| fun () ->
        it "AM designators" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 3)
            Date.Format.format "tt" testDate
            |> equal "AM"

        it "PM designators" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 3)
            Date.Format.format "tt" testDate
            |> equal "PM"

        it "Empty AM designators works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.localFormat localeFrench "tt" testDate
            |> equal ""

        it "Empty PM designators works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 15, 0, 33)
            Date.Format.localFormat localeFrench "tt" testDate
            |> equal ""

    describe "Pattern 'y': The year, from 0 to 99" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format "y" testDate
            |> equal "1"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format "y" testDate
            |> equal "0"

        it "4 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(1900, 1, 1, 0, 0, 0)
            Date.Format.format "y" testDate
            |> equal "0"

        it "4 digit year ending with non zero (0-9) works" <| fun () ->
            let testDate = DateTime(2009, 1, 1, 0, 0, 0)
            Date.Format.format "y" testDate
            |> equal "9"

        it "4 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format "y" testDate
            |> equal "19"

    describe "Pattern 'yy': The year, from 00 to 99" <| fun () ->
        it "1 digit year ending with non zero (0-9) works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format "yy" testDate
            |> equal "01"

        it "2 digit year ending with 0 works" <| fun () ->
            let testDate = DateTime.Parse("0010-01-01")
            Date.Format.format "yy" testDate
            |> equal "10"

        it "2 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format "yy" testDate
            |> equal "01"

        it "2 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime.Parse("0011-01-01")
            Date.Format.format "yy" testDate
            |> equal "11"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format "yy" testDate
            |> equal "00"

        it "3 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime(905, 1, 1, 0, 0, 0)
            Date.Format.format "yy" testDate
            |> equal "05"

        it "3 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(911, 1, 1, 0, 0, 0)
            Date.Format.format "yy" testDate
            |> equal "11"

        it "4 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(1900, 1, 1, 0, 0, 0)
            Date.Format.format "yy" testDate
            |> equal "00"

        it "4 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime(2009, 1, 1, 0, 0, 0)
            Date.Format.format "yy" testDate
            |> equal "09"

        it "4 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format "yy" testDate
            |> equal "19"

    describe "Pattern 'yyy': The year, with a minimum of three digits" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format "yyy" testDate
            |> equal "001"

        it "2 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0011-01-01")
            Date.Format.format "yyy" testDate
            |> equal "011"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format "yyy" testDate
            |> equal "900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format "yyy" testDate
            |> equal "2019"

    describe "Pattern 'yyyy': The year as a four-digit number" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format "yyyy" testDate
            |> equal "0001"

        it "2 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0011-01-01")
            Date.Format.format "yyyy" testDate
            |> equal "0011"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format "yyyy" testDate
            |> equal "0900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format "yyyy" testDate
            |> equal "2019"

    describe "Pattern 'yyyyy': The year as a five-digit number" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            Date.Format.format "yyyyy" testDate
            |> equal "00001"

        it "2 digit year works" <| fun () ->
            let testDate = DateTime.Parse("0099-01-01")
            Date.Format.format "yyyyy" testDate
            |> equal "00099"

        it "3 digit year works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            Date.Format.format "yyyyy" testDate
            |> equal "00900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            Date.Format.format "yyyyy" testDate
            |> equal "02019"

    describe "Escaping character work" <| fun () ->
        it "Escape work" <| fun _ ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.format "yyyy-MM-dd hh:mm:ss \d \y \M\M" testDate
            |> equal "2017-08-22 01:00:33 d y MM"

    describe "Localization" <| fun _ ->
        let testDate = DateTime(2017, 8, 22, 1, 0, 33)

        let formatEnglish = Date.Format.localFormat localeEnglish
        let formatFrench = Date.Format.localFormat localeFrench
        let formatRussian = Date.Format.localFormat localeRussian
        let formatHungarian = Date.Format.localFormat localeHungarian

        it "French works" <| fun _ ->
            formatFrench "dddd" testDate
            |> equal "Mardi"

        it "English works" <| fun _ ->
            formatEnglish "dddd" testDate
            |> equal "Tuesday"

        it "Russian works" <| fun _ ->
            formatRussian "dddd" testDate
            |> equal "Вторник"

        it "Hungarian works" <| fun _ ->
            formatHungarian "dddd" testDate
            |> equal "Kedd"

        describe "Default date formats" <| fun _ ->
            it "French works" <| fun _ ->
                formatFrench localeFrench.Date.DefaultFormat testDate
                |> equal "22/8/2017"

            it "English works" <| fun _ ->
                formatEnglish localeEnglish.Date.DefaultFormat testDate
                |> equal "22/8/2017"

            it "Russian works" <| fun _ ->
                formatRussian localeRussian.Date.DefaultFormat testDate
                |> equal "22.8.2017"

            it "Hungarian works" <| fun _ ->
                formatHungarian localeHungarian.Date.DefaultFormat testDate
                |> equal "2017.08.22."
