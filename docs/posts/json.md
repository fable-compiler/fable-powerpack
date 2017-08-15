---
layout: post
title: Json
---

The Json utility allow you to parse any Json object via pattern matching.

Example:

```fs
type UserDef =
    { firstname : string
      country : string }

type JsonDef =
    { size : int
      users : UserDef list }

    let jsonTxt = 
        """
{
    size: 3
    users: [
        {
            firstname: "Maxime",
            country: "France"
        },
        {
            firstname: "Alfonso",
            country: "Spain"
        },
        {
            firstname: "Forki",
            country: "Germany"
        }
    ]
}        
        """

    let unwrap e =
        match e with
        | Ok o -> o
        | Error (exn: System.Exception) -> failwith exn.Message

    let userList =
        match ofString jsonTxt |> unwrap with
        | Json.Object [| ("size", Json.Number sizeValue)
                         ("users", userList) |] ->
            match (userList) with
            | Json.Array users ->
                let userList =
                    [ for user in users do
                        yield match user with
                              | Json.Object a ->
                                  let obj = Map.ofArray a
                                  let firstname = obj |> Map.find "firstname"
                                  let country = obj |> Map.find "country"
                                  match (firstname, country) with
                                  | (Json.String firstnameValue, Json.String countryValue ) ->
                                      { firstname = firstnameValue
                                        country = countryValue }
                                  | _ -> failwith "Invalid type"
                              | _ -> failwith "Invalid  type"
                    ]

                { size = int sizeValue
                  users = userList }
            | _ -> failwith "Invalid type"
        | _ -> failwith "Invalid json"
```