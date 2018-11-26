namespace Fable.PowerPack.Date

module Format =

    open Fable.PowerPack.Date.Local
    open System
    open System.Text.RegularExpressions

    let internal fromDayOfWeek local day =
        match day with
        | DayOfWeek.Monday -> local.Monday
        | DayOfWeek.Tuesday -> local.Tuesday
        | DayOfWeek.Wednesday -> local.Wednesday
        | DayOfWeek.Thursday -> local.Thursday
        | DayOfWeek.Friday -> local.Friday
        | DayOfWeek.Saturday -> local.Saturday
        | DayOfWeek.Sunday -> local.Sunday
        | x -> failwithf "Not a valid day of week: %A" x

    let internal mod12 x = if x = 12 || x = 0 then 12 else x % 12

    let internal fromMonth local month =
        match month with
        | 1 -> local.January
        | 2 -> local.February
        | 3 -> local.March
        | 4 -> local.April
        | 5 -> local.May
        | 6 -> local.June
        | 7 -> local.July
        | 8 -> local.August
        | 9 -> local.September
        | 10  -> local.October
        | 11 -> local.November
        | 12 -> local.December
        | x -> failwithf "Not a valid month rank: %i" x

    let internal toString x = string x

    let inline internal padWithN n c =  (fun (x: string) -> x.PadLeft(n, c)) << string

    let internal padWith = padWithN 2

    let internal takeLastChars count (str : string) =
        str.Substring(Math.Max(0, str.Length - count))

    let localFormat (local : Localization) formatString (date : DateTime) =
        Regex.Replace(
            formatString,
            @"(d{1,4})|(h{1,2})|(H{1,2})|(m{1,2})|(M{1,4})|(s{1,2})|(t{1,2})|(y{1,5})|(\\.?)",
            MatchEvaluator((fun token ->
                let symbol = token.Groups.[0]
                // If we escape the next character
                if symbol.Value.StartsWith("\\") && symbol.Value.Length = 2 then
                    symbol.Value.Substring(1)
                else
                    match symbol.Value with
                    | "d" -> date.Day |> toString
                    | "dd" -> date.Day |> toString |> padWith '0'
                    | "ddd" -> date.DayOfWeek |> fromDayOfWeek local.Date.AbbreviatedDays
                    | "dddd" -> date.DayOfWeek |> fromDayOfWeek local.Date.Days
                    | "h" -> date.Hour |> mod12 |> toString
                    | "hh" -> date.Hour |> mod12 |> toString |> padWith '0'
                    | "H" -> date.Hour |> toString
                    | "HH" -> date.Hour |> toString |> padWith '0'
                    | "m" -> date.Minute |> toString
                    | "mm" -> date.Minute |> toString |> padWith '0'
                    | "M" -> date.Month |> toString
                    | "MM" -> date.Month |> toString |> padWith '0'
                    | "MMM" -> date.Month |> fromMonth local.Date.AbbreviatedMonths
                    | "MMMM" -> date.Month |> fromMonth local.Date.Months
                    | "s" -> date.Second |> toString
                    | "ss" -> date.Second |> toString |> padWith '0'
                    | "t" ->
                        try
                            if date.Hour < 12 then
                                local.Time.AM.Substring(0, 1)
                            else
                                local.Time.PM.Substring(0, 1)
                        with
                        | _ -> ""
                    | "tt" -> if date.Hour < 12 then local.Time.AM else local.Time.PM
                    | "y" ->
                        let year = date.Year |> toString |> takeLastChars 2
                        if year.Chars(0) = '0' then
                            year.Substring(1)
                        else
                            year
                    | "yy" -> date.Year |> toString |> takeLastChars 2 |> padWith '0'
                    | "yyy" -> date.Year |> toString |> padWithN 3 '0'
                    | "yyyy" -> date.Year |> toString |> padWithN 4 '0'
                    | "yyyyy" -> date.Year |> toString |> padWithN 5 '0'
                    | t -> failwithf "The token %s is not implemented. Please report it" t
            ))
        )
