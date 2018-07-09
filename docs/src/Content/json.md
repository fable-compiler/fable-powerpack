# Json

The Json utility allow you to parse any Json object via pattern matching.

You can either use classic function style or computation expression to parse your Json.

For next samples we will consider the following types:

```fs
type UserDef =
    { firstname : string
      country : string }

type JsonDef =
    { size : int
      users : UserDef list }
```

The json to parse is:

```fs
let jsonTxt =
    """
{
    "size": 3,
    "users": [
        {
            "firstname": "Maxime",
            "country": "France"
        },
        {
            "firstname": "Alfonso",
            "country": "Spain"
        },
        {
            "firstname": "Forki",
            "country": "Germany"
        }
    ]
}
    """
```

## Using standard function

```fs

// Helpers function to unwrap a Json value
let unwrapNumber a =
    match a with
    | Json.Number a -> a
    | _ -> failwith "Invalid JSON, it must be a number"

let unwrapString a =
    match a with
    | Json.String a -> a
    | _ -> failwith "Invalid JSON, it must be a string"

let unwrapObject a =
    match a with
    | Json.Object a -> Map.ofArray a
    | _ -> failwith "Invalid JSON, it must be an object"

let unwrapArray a =
    match a with
    | Json.Array a -> a
    | _ -> failwith "Invalid JSON, it must be an array"

// Convert the Json string into Json DU
let json = jsonTxt |> Json.ofString |> unwrapResult |> unwrapObject

let sizeValue = json |> Map.find "size" |> unwrapNumber
let users = json |> Map.find "users" |> unwrapArray

let userList =
    [ for user in users do
        let obj = user |> unwrapObject
        let firstname = obj |> Map.find "firstname" |> unwrapString
        let country = obj |> Map.find "country" |> unwrapString
        yield { firstname = firstname
                country = country }
    ]

{ size = int sizeValue
  users = userList } // Here we have our json extracted
```

## Using computation expression

```fs

// Helpers to extract the Json values into a Result type
let number a =
    match a with
    | Json.Number a -> Ok(a)
    | _ -> Error(System.Exception("Invalid JSON, it must be a number"))

let string a =
    match a with
    | Json.String a -> Ok(a)
    | _ -> Error(System.Exception("Invalid JSON, it must be a string"))

let lookup (key: string) (a: Map<string, Json.Json>) =
    match Map.tryFind key a with
    | Some(a) -> Ok(a)
    | None -> Error(System.Exception("Could not find key " + key))

let object a =
    match a with
    | Json.Object a -> Ok(Map.ofArray a)
    | _ -> Error(System.Exception("Invalid JSON, it must be an object"))

let array a =
    match a with
    | Json.Array a -> Ok(a)
    | _ -> Error(System.Exception("Invalid JSON, it must be an array"))


result {
    let! json = jsonTxt |> Json.ofString |> Result.bind object

    let! sizeValue = json |> lookup "size" |> Result.bind number
    let! users = json |> lookup "users" |> Result.bind array

    let userList =
        seq {
            for user in users ->
                result {
                    let! obj = user |> object
                    let! firstname = obj |> lookup "firstname" |> Result.bind string
                    let! country = obj |> lookup "country" |> Result.bind string
                    return { firstname = firstname
                             country = country }
                }
        }
        // Make sure all the users are valid
        |> Seq.map unwrapResult

    return { size = int sizeValue
             users = List.ofSeq userList }
}
// We unwrap the result
// Make sure the parser succeeded
|> unwrapResult
```
