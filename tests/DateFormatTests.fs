module DateFormatTests

open System
open Fable.Core
open Fable.PowerPack

let inline equal (expected: 'T) (actual: 'T): unit =
    Testing.Assert.AreEqual(expected, actual)

[<Global>]
let it (_msg: string) (f: unit->unit): unit = jsNative

[<Global>]
let describe (_msg: string) (f: unit->unit): unit = jsNative

describe "DateFormat tests" <| fun _ ->
    let localEnglishUK = Date.Local.englishUK
    let localEnglishUS = Date.Local.englishUS
    let localFrench = Date.Local.french
    let localRussian = Date.Local.russian
    let localHungarian = Date.Local.hungarian

    let formatUK formatString (date : DateTime) = Date.Format.localFormat Date.Local.englishUK formatString date

    describe "Pattern 'd': The day of the month, from 1 through 31" <| fun () ->
        it "1 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            formatUK "d" testDate
            |> equal "8"

        it "2 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 0)
            formatUK "d" testDate
            |> equal "22"

    describe "Pattern 'dd': The day of the month, from 01 through 31" <| fun () ->
        it "1 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            formatUK "dd" testDate
            |> equal "08"

        it "2 digit day of month works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 0)
            formatUK "dd" testDate
            |> equal "22"

    describe "Pattern 'ddd': The abbreviated name of the day of the week" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            formatUK "ddd" testDate
            |> equal "Tue"

    describe "Pattern 'dddd': The full name of the day of the week" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            formatUK "dddd" testDate
            |> equal "Tuesday"

    describe "Pattern 'h': The hour, using a 12-hour clock from 1 to 12" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            formatUK "h" testDate
            |> equal "1"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            formatUK "h" testDate
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            formatUK "h" testDate
            |> equal "3"

        it "Noon works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 12, 0, 0)
            formatUK "h" testDate
            |> equal "12"

        it "Midnight works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            formatUK "h" testDate
            |> equal "12"

    describe "Pattern 'hh': The hour, using a 12-hour clock from 01 to 12" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            formatUK "hh" testDate
            |> equal "01"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            formatUK "hh" testDate
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            formatUK "hh" testDate
            |> equal "03"

        it "Noon works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 12, 0, 0)
            formatUK "hh" testDate
            |> equal "12"

        it "Midnight works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 0)
            formatUK "hh" testDate
            |> equal "12"

    describe "Pattern 'H': The hour, using a 24-hour clock from 0 to 23" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            formatUK "H" testDate
            |> equal "1"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            formatUK "H" testDate
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            formatUK "H" testDate
            |> equal "15"

    describe "Pattern 'HH': The hour, using a 24-hour clock from 00 to 23" <| fun () ->
        it "1 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 0)
            formatUK "HH" testDate
            |> equal "01"

        it "2 digit hour works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 11, 0, 0)
            formatUK "HH" testDate
            |> equal "11"

        it "PM hours works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 0)
            formatUK "HH" testDate
            |> equal "15"

    describe "Pattern 'm': The minute, from 0 through 59" <| fun () ->
        it "1 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            formatUK "m" testDate
            |> equal "4"

        it "2 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 22, 0)
            formatUK "m" testDate
            |> equal "22"

    describe "Pattern 'mm': The minute, from 00 through 59" <| fun () ->
        it "1 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            formatUK "mm" testDate
            |> equal "04"

        it "2 digit minute works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 22, 0)
            formatUK "mm" testDate
            |> equal "22"

    describe "Pattern 'M': The month, from 1 through 12" <| fun () ->
        it "1 digit month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            formatUK "M" testDate
            |> equal "8"

        it "2 digit month works" <| fun () ->
            let testDate = DateTime(2017, 11, 22, 0, 22, 0)
            formatUK "M" testDate
            |> equal "11"

    describe "Pattern 'MM': The month, from 01 through 12" <| fun () ->
        it "1 digit month works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            formatUK "MM" testDate
            |> equal "08"

        it "2 digit month works" <| fun () ->
            let testDate = DateTime(2017, 11, 22, 0, 22, 0)
            formatUK "MM" testDate
            |> equal "11"

    describe "Pattern 'MMM': The abbreviated name of the month" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            formatUK "MMM" testDate
            |> equal "Aug"

    describe "Pattern 'MMMM': The full name of the month" <| fun () ->
        it "Format works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 4, 0)
            formatUK "MMMM" testDate
            |> equal "August"

    describe "Pattern 's': The second, from 0 through 59" <| fun () ->
        it "1 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 3)
            formatUK "s" testDate
            |> equal "3"

        it "2 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 33)
            formatUK "s" testDate
            |> equal "33"

    describe "Pattern 'ss': The second, from 00 through 59" <| fun () ->
        it "1 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 0, 0, 3)
            formatUK "ss" testDate
            |> equal "03"

        it "2 digit second works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 0, 0, 33)
            formatUK "ss" testDate
            |> equal "33"

    describe "Pattern 't': The first character of the AM/PM designator" <| fun () ->
        it "AM designators with 2 or more letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 3)
            formatUK "t" testDate
            |> equal "A"

        it "PM designators with 2 or more letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 3)
            formatUK "t" testDate
            |> equal "P"

        it "AM designators with less then 2 letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.localFormat localFrench "t" testDate
            |> equal ""

        it "PM designators with less then 2 letters works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 15, 0, 33)
            Date.Format.localFormat localFrench "t" testDate
            |> equal ""

    describe "Pattern 'tt': The AM/PM designator" <| fun () ->
        it "AM designators" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 1, 0, 3)
            formatUK "tt" testDate
            |> equal "AM"

        it "PM designators" <| fun () ->
            let testDate = DateTime(2017, 8, 8, 15, 0, 3)
            formatUK "tt" testDate
            |> equal "PM"

        it "Empty AM designators works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            Date.Format.localFormat localFrench "tt" testDate
            |> equal ""

        it "Empty PM designators works" <| fun () ->
            let testDate = DateTime(2017, 8, 22, 15, 0, 33)
            Date.Format.localFormat localFrench "tt" testDate
            |> equal ""

    describe "Pattern 'y': The year, from 0 to 99" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            formatUK "y" testDate
            |> equal "1"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            formatUK "y" testDate
            |> equal "0"

        it "4 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(1900, 1, 1, 0, 0, 0)
            formatUK "y" testDate
            |> equal "0"

        it "4 digit year ending with non zero (0-9) works" <| fun () ->
            let testDate = DateTime(2009, 1, 1, 0, 0, 0)
            formatUK "y" testDate
            |> equal "9"

        it "4 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            formatUK "y" testDate
            |> equal "19"

    describe "Pattern 'yy': The year, from 00 to 99" <| fun () ->
        it "1 digit year ending with non zero (0-9) works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            formatUK "yy" testDate
            |> equal "01"

        it "2 digit year ending with 0 works" <| fun () ->
            let testDate = DateTime.Parse("0010-01-01")
            formatUK "yy" testDate
            |> equal "10"

        it "2 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime.Parse("0001-01-01")
            formatUK "yy" testDate
            |> equal "01"

        it "2 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime.Parse("0011-01-01")
            formatUK "yy" testDate
            |> equal "11"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            formatUK "yy" testDate
            |> equal "00"

        it "3 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime(905, 1, 1, 0, 0, 0)
            formatUK "yy" testDate
            |> equal "05"

        it "3 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(911, 1, 1, 0, 0, 0)
            formatUK "yy" testDate
            |> equal "11"

        it "4 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(1900, 1, 1, 0, 0, 0)
            formatUK "yy" testDate
            |> equal "00"

        it "4 digit year ending with non zero (1-9) works" <| fun () ->
            let testDate = DateTime(2009, 1, 1, 0, 0, 0)
            formatUK "yy" testDate
            |> equal "09"

        it "4 digit year ending with non zero (11-99) works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            formatUK "yy" testDate
            |> equal "19"

    describe "Pattern 'yyy': The year, with a minimum of three digits" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            formatUK "yyy" testDate
            |> equal "001"

        it "2 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0011-01-01")
            formatUK "yyy" testDate
            |> equal "011"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            formatUK "yyy" testDate
            |> equal "900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            formatUK "yyy" testDate
            |> equal "2019"

    describe "Pattern 'yyyy': The year as a four-digit number" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            formatUK "yyyy" testDate
            |> equal "0001"

        it "2 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0011-01-01")
            formatUK "yyyy" testDate
            |> equal "0011"

        it "3 digit year ending with 00 works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            formatUK "yyyy" testDate
            |> equal "0900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            formatUK "yyyy" testDate
            |> equal "2019"

    describe "Pattern 'yyyyy': The year as a five-digit number" <| fun () ->
        it "1 digit year works" <| fun () ->
            // Force the date to be Year 0001
            // JavaScript consider year from 00-99 to be in 1900-1999
            let testDate = DateTime.Parse("0001-01-01")
            formatUK "yyyyy" testDate
            |> equal "00001"

        it "2 digit year works" <| fun () ->
            let testDate = DateTime.Parse("0099-01-01")
            formatUK "yyyyy" testDate
            |> equal "00099"

        it "3 digit year works" <| fun () ->
            let testDate = DateTime(900, 1, 1, 0, 0, 0)
            formatUK "yyyyy" testDate
            |> equal "00900"

        it "4 digit year works" <| fun () ->
            let testDate = DateTime(2019, 1, 1, 0, 0, 0)
            formatUK "yyyyy" testDate
            |> equal "02019"

    describe "Escaping character work" <| fun () ->
        it "Escape work" <| fun _ ->
            let testDate = DateTime(2017, 8, 22, 1, 0, 33)
            formatUK "yyyy-MM-dd hh:mm:ss \d \y \M\M" testDate
            |> equal "2017-08-22 01:00:33 d y MM"

    describe "Localization" <| fun _ ->
        let testDate = DateTime(2017, 8, 22, 1, 0, 33)

        let formatEnglishUK = Date.Format.localFormat localEnglishUK
        let formatEnglishUS = Date.Format.localFormat localEnglishUS
        let formatFrench = Date.Format.localFormat localFrench
        let formatRussian = Date.Format.localFormat localRussian
        let formatHungarian = Date.Format.localFormat localHungarian

        describe "Day names" <| fun _ ->
            it "French works" <| fun _ ->
                formatFrench "dddd" testDate
                |> equal "Mardi"

            it "English UK works" <| fun _ ->
                formatEnglishUK "dddd" testDate
                |> equal "Tuesday"

            it "English US works" <| fun _ ->
                formatEnglishUS "dddd" testDate
                |> equal "Tuesday"

            it "Russian works" <| fun _ ->
                formatRussian "dddd" testDate
                |> equal "Вторник"

            it "Hungarian works" <| fun _ ->
                formatHungarian "dddd" testDate
                |> equal "Kedd"

        describe "Default date formats" <| fun _ ->
            it "French works" <| fun _ ->
                let formatDateFrench = Date.Format.localFormat localFrench localFrench.Date.DefaultFormat
                formatDateFrench testDate
                |> equal "22/8/2017"

            it "English UK works" <| fun _ ->
                let formatDateUK = Date.Format.localFormat localEnglishUK localEnglishUK.Date.DefaultFormat
                formatDateUK testDate
                |> equal "22/8/2017"

            it "English US works" <| fun _ ->
                let formatDateUS = Date.Format.localFormat localEnglishUS localEnglishUS.Date.DefaultFormat
                formatDateUS testDate
                |> equal "8/22/2017"

            it "Russian works" <| fun _ ->
                let formatDateRussian = Date.Format.localFormat localRussian localRussian.Date.DefaultFormat
                formatDateRussian testDate
                |> equal "22.8.2017"

            it "Hungarian works" <| fun _ ->
                let formatDateHungarian = Date.Format.localFormat localHungarian localHungarian.Date.DefaultFormat
                formatDateHungarian testDate
                |> equal "2017.08.22."
