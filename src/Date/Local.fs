namespace Fable.PowerPack.Date

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
          FirstDayOfTheWeek : int }

    type Time =
        { AM : string
          PM : string }

    type Localization =
        { Date : Date
          Time : Time }

    let english =
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
              FirstDayOfTheWeek = 1 }
          Time =
            { AM = "AM"
              PM = "PM" } }

    let french =
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
              FirstDayOfTheWeek = 1 }
          Time =
            { AM = ""
              PM = "" } }

    let russian =
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
              FirstDayOfTheWeek = 1 }
          Time =
            { AM = ""
              PM = "" } }

    let hungarian =
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
              FirstDayOfTheWeek = 1 }
          Time =
            { AM = "de"
              PM = "du" } }
