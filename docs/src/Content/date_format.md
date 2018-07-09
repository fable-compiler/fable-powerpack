# Date Format

## Basic

Date module allow you to format a date according to a pattern.


```fs
open Fable.PowerPack

Date.Format.localFormat Date.Local.englishUK "yyyy-MM-dd hh:mm:ss" testDate
// Result: "2017-08-22 01:00:33"

Date.Format.localFormat Date.Local.englishUK "dddd, MMMM dd, yyyy" testDate
// Result: "Tuesday, August 22, 2017"
```

## Localization

```fs
open Fable.PowerPack

Date.Format.localFormat Date.Local.french "dddd, dd MMMM yyyy" testDate
// Result: "Mardi, 22 août 2017"

// Use default date format:
Date.Format.localFormat Date.Local.french Date.Local.french.Date.DefaultFormat testDate
// Result: "22/3/2017"

Date.Format.localFormat Date.Local.englishUS Date.Local.englishUS.Date.DefaultFormat testDate
// Result: "3/22/2017"
```

## Supported formats

Here is all the pattern supported and their description. This patterns are from .Net so you can find more information [here](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings).

<table class="table is-narrow is-striped">
    <thead>
        <tr>
            <th>Pattern</th>
            <th>Description</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>d</td>
            <td>The day of the month, from 1 through 31</td>
        </tr>
        <tr>
            <td>dd</td>
            <td>The day of the month, from 01 through 31</td>
        </tr>
        <tr>
            <td>ddd</td>
            <td>The abbreviated name of the day of the week</td>
        </tr>
        <tr>
            <td>dddd</td>
            <td>The full name of the day of the week</td>
        </tr>
        <tr>
            <td>h</td>
            <td>The hour, using a 12-hour clock from 1 to 12</td>
        </tr>
        <tr>
            <td>hh</td>
            <td>The hour, using a 12-hour clock from 01 to 12</td>
        </tr>
        <tr>
            <td>H</td>
            <td>The hour, using a 24-hour clock from 0 to 23</td>
        </tr>
        <tr>
            <td>HH</td>
            <td>The hour, using a 24-hour clock from 00 to 23</td>
        </tr>
        <tr>
            <td>m</td>
            <td>The minute, from 0 through 59</td>
        </tr>
        <tr>
            <td>mm</td>
            <td>The minute, from 00 through 59</td>
        </tr>
        <tr>
            <td>M</td>
            <td>The month, from 1 through 12</td>
        </tr>
        <tr>
            <td>MM</td>
            <td>The month, from 01 through 12</td>
        </tr>
        <tr>
            <td>MMM</td>
            <td>The abbreviated name of the month</td>
        </tr>
        <tr>
            <td>MMMM</td>
            <td>The full name of the month</td>
        </tr>
        <tr>
            <td>s</td>
            <td>The second, from 00 through 59</td>
        </tr>
        <tr>
            <td>ss</td>
            <td>The second, from 00 through 59</td>
        </tr>
        <tr>
            <td>t</td>
            <td>The first character of the AM/PM designator</td>
        </tr>
        <tr>
            <td>tt</td>
            <td>The AM/PM designator</td>
        </tr>
        <tr>
            <td>y</td>
            <td>The year, from 0 to 99</td>
        </tr>
        <tr>
            <td>yy</td>
            <td>The year, from 00 to 99</td>
        </tr>
        <tr>
            <td>yyy</td>
            <td>The year, with a minimum of three digits</td>
        </tr>
        <tr>
            <td>yyyy</td>
            <td>The year as a four-digit number</td>
        </tr>
        <tr>
            <td>yyyyy</td>
            <td>The year as a five-digit number</td>
        </tr>
        <tr>
            <td>\</td>
            <td>Escape the next character</td>
        </tr>
    </tbody>
</table>
