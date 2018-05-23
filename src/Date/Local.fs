namespace Fable.PowerPack.Date
open System

module Local =

    type Months =
        { January : string
          February : string
          March : string
          April : string
          May : string
          June : string
          July : string
          August : string
          September : string
          October : string
          November : string
          December : string }

    type DaysOfWeek =
        { Monday : string
          Tuesday : string
          Wednesday : string
          Thursday : string
          Friday : string
          Saturday : string
          Sunday : string }

    type Date =
        { Months : Months
          AbbreviatedMonths : Months
          Days : DaysOfWeek
          AbbreviatedDays : DaysOfWeek
          DefaultFormat : string
          FirstDayOfTheWeek : DayOfWeek }

    type Time =
        { AM : string
          PM : string }

    type Localization =
        { Date : Date
          Time : Time }

    let englishUK<'T> =
        { Date =
            { Months =
                { January = "January"
                  February = "February"
                  March = "March"
                  April = "April"
                  May = "May"
                  June = "June"
                  July = "July"
                  August = "August"
                  September = "September"
                  October = "October"
                  November = "November"
                  December = "December" }
              AbbreviatedMonths =
                { January = "Jan"
                  February = "Feb"
                  March = "Mar"
                  April = "Apr"
                  May = "May"
                  June = "Jun"
                  July = "Jul"
                  August = "Aug"
                  September = "Sep"
                  October = "Oct"
                  November = "Nov"
                  December = "Dec" }
              Days =
                { Monday = "Monday"
                  Tuesday = "Tuesday"
                  Wednesday = "Wednesday"
                  Thursday = "Thursday"
                  Friday = "Friday"
                  Saturday = "Saturday"
                  Sunday = "Sunday" }
              AbbreviatedDays =
                { Monday = "Mon"
                  Tuesday = "Tue"
                  Wednesday = "Wed"
                  Thursday = "Thu"
                  Friday = "Fri"
                  Saturday = "Sat"
                  Sunday = "Sun" }
              DefaultFormat = "d/M/yyyy"
              FirstDayOfTheWeek = DayOfWeek.Monday }
          Time =
            { AM = "AM"
              PM = "PM" } }

    let englishUS<'T> =
        { Date =
            { Months =
                { January = "January"
                  February = "February"
                  March = "March"
                  April = "April"
                  May = "May"
                  June = "June"
                  July = "July"
                  August = "August"
                  September = "September"
                  October = "October"
                  November = "November"
                  December = "December" }
              AbbreviatedMonths =
                { January = "Jan"
                  February = "Feb"
                  March = "Mar"
                  April = "Apr"
                  May = "May"
                  June = "Jun"
                  July = "Jul"
                  August = "Aug"
                  September = "Sep"
                  October = "Oct"
                  November = "Nov"
                  December = "Dec" }
              Days =
                { Monday = "Monday"
                  Tuesday = "Tuesday"
                  Wednesday = "Wednesday"
                  Thursday = "Thursday"
                  Friday = "Friday"
                  Saturday = "Saturday"
                  Sunday = "Sunday" }
              AbbreviatedDays =
                { Monday = "Mon"
                  Tuesday = "Tue"
                  Wednesday = "Wed"
                  Thursday = "Thu"
                  Friday = "Fri"
                  Saturday = "Sat"
                  Sunday = "Sun" }
              DefaultFormat = "M/d/yyyy"
              FirstDayOfTheWeek = DayOfWeek.Sunday }
          Time =
            { AM = "AM"
              PM = "PM" } }

    let french<'T> =
        { Date =
            { Months =
                { January = "Janvier"
                  February = "Février"
                  March = "Mars"
                  April = "Avril"
                  May = "Mai"
                  June = "Juin"
                  July = "Juillet"
                  August = "Août"
                  September = "Septembre"
                  October = "Octobre"
                  November = "Novembre"
                  December = "Décembre" }
              AbbreviatedMonths =
                { January = "Jan"
                  February = "Fév"
                  March = "Mars"
                  April = "Avr"
                  May = "Mai"
                  June = "Jui"
                  July = "Juil"
                  August = "Août"
                  September = "Sep"
                  October = "Oct"
                  November = "Nov"
                  December = "Dec" }
              Days =
                { Monday = "Lundi"
                  Tuesday = "Mardi"
                  Wednesday = "Mercredi"
                  Thursday = "Jeudi"
                  Friday = "Vendredi"
                  Saturday = "Samedi"
                  Sunday = "Dimanche" }
              AbbreviatedDays =
                { Monday = "Lun"
                  Tuesday = "Mar"
                  Wednesday = "Mer"
                  Thursday = "Jeu"
                  Friday = "Ven"
                  Saturday = "Sam"
                  Sunday = "Dim" }
              DefaultFormat = "d/M/yyyy"
              FirstDayOfTheWeek = DayOfWeek.Monday }
          Time =
            { AM = ""
              PM = "" } }

    let russian<'T> =
        { Date =
            { Months =
                { January = "Январь"
                  February = "Февраль"
                  March = "Март"
                  April = "Апрель"
                  May = "Май"
                  June = "Июнь"
                  July = "Июль"
                  August = "Август"
                  September = "Сентябрь"
                  October = "Октябрь"
                  November = "Ноябрь"
                  December = "Декабрь" }
              AbbreviatedMonths =
                { January = "Янв"
                  February = "Фев"
                  March = "Мар"
                  April = "Апр"
                  May = "Май"
                  June = "Июн"
                  July = "Июл"
                  August = "Авг"
                  September = "Сен"
                  October = "Окт"
                  November = "Ноя"
                  December = "Дек" }
              Days =
                { Monday = "Понедельник"
                  Tuesday = "Вторник"
                  Wednesday = "Среда"
                  Thursday = "Четверг"
                  Friday = "Пятница"
                  Saturday = "Суббота"
                  Sunday = "Воскресенье" }
              AbbreviatedDays =
                { Monday = "Пон"
                  Tuesday = "Втр"
                  Wednesday = "Срд"
                  Thursday = "Чтв"
                  Friday = "Птн"
                  Saturday = "Сбт"
                  Sunday = "Вск" }
              DefaultFormat = "d.M.yyyy"
              FirstDayOfTheWeek = DayOfWeek.Monday }
          Time =
            { AM = ""
              PM = "" } }

    let hungarian<'T> =
        { Date =
            { Months =
                { January = "Január"
                  February = "Február"
                  March = "Március"
                  April = "Április"
                  May = "Május"
                  June = "Június"
                  July = "Július"
                  August = "Augusztus"
                  September = "Szeptember"
                  October = "Október"
                  November = "November"
                  December = "December" }
              AbbreviatedMonths =
                { January = "Jan"
                  February = "Feb"
                  March = "Márc"
                  April = "Ápr"
                  May = "Máj"
                  June = "Jún"
                  July = "Júl"
                  August = "Aug"
                  September = "Szept"
                  October = "Okt"
                  November = "Nov"
                  December = "Dec" }
              Days =
                { Monday = "Hétfő"
                  Tuesday = "Kedd"
                  Wednesday = "Szerda"
                  Thursday = "Csütörtök"
                  Friday = "Péntek"
                  Saturday = "Szombat"
                  Sunday = "Vasárnap" }
              AbbreviatedDays =
                { Monday = "Hét"
                  Tuesday = "Kedd"
                  Wednesday = "Sze"
                  Thursday = "Csüt"
                  Friday = "Pén"
                  Saturday = "Szo"
                  Sunday = "Vas" }
              DefaultFormat = "yyyy.MM.dd."
              FirstDayOfTheWeek = DayOfWeek.Monday }
          Time =
            { AM = "de"
              PM = "du" } }


    let german<'T> =
        { Date =
            { Months =
                { January = "Januar"
                  February = "Februar"
                  March = "März"
                  April = "April"
                  May = "Mai"
                  June = "Juni"
                  July = "Juli"
                  August = "August"
                  September = "September"
                  October = "Oktober"
                  November = "November"
                  December = "Dezember" }
              AbbreviatedMonths =
                { January = "Jan"
                  February = "Feb"
                  March = "März"
                  April = "Apr"
                  May = "Mai"
                  June = "Juni"
                  July = "Juli"
                  August = "Aug"
                  September = "Sept"
                  October = "Okt"
                  November = "Nov"
                  December = "Dez" }
              Days =
                { Monday = "Montag"
                  Tuesday = "Dienstag"
                  Wednesday = "Mittwoch"
                  Thursday = "Donnerstag"
                  Friday = "Freitag"
                  Saturday = "Samstag"
                  Sunday = "Sonntag" }
              AbbreviatedDays =
                { Monday = "Mo"
                  Tuesday = "Di"
                  Wednesday = "Mi"
                  Thursday = "Do"
                  Friday = "Fr"
                  Saturday = "Sa"
                  Sunday = "So" }
              DefaultFormat = "dd.MM.yyyy"
              FirstDayOfTheWeek = DayOfWeek.Monday }
          Time =
            { AM = ""
              PM = "" } }
