---
layout: post
title: Fetch
---

Fetch is an API making request over network easier than with `XMLHttpRequest`. It's allow you to write your request using "pipeline" style.

## Access

To acces the fetch API, you need to use the following `open` declaration

```fs
open Fable.PowerPack
open Fable.PowerPack.Fetch
```

## Basic

In the next snippet, we are getting the content of `http://fable.io` page.

```fs
    fetch "http://fable.io" []
    |> Promise.bind (fun res -> res.text())
    |> Promise.map (fun txt ->
        // Access here your ressource
        Browser.console.log txt
    )
```

We can write the same request using **Promise computation**.

```fs
    promise {
        let! res = fetch "http://fable.io" []
        let! txt = res.text()
        // Access here your ressource
        Browser.console.log txt
    }
```

## Send a POST request with data

```fs
    // This is the data we want to send to the server
    let data = {
        Username: "Maxime"
        Password: "myPassword"
    }

    let defaultProps =
        [ RequestProperties.Method HttpMethod.POST
        ; requestHeaders [ContentType "application/json"]
        ; RequestProperties.Body unbox(toJson data)]
    
    promise {
        let! res = fetch "http://my-server.com/sign-in" defaultProps
        let txt = res.text()
        // Here you got access to the server response
        if txt = "true" then
            // Sign in: OK
        else
            // Sign in: Failed
    }    
```

Fable Powerpack already implement some general helpers for you like:

- postRecord: Sends a HTTP post with the record serialized as JSON.
- tryPostRecord: Sends a HTTP post with the record serialized as JSON. And encapsulate it into a `Result` case.
- putRecord: Sends a HTTP put with the record serialized as JSON.
- etc.