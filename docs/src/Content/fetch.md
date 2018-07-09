# Fetch

Fetch is an API making a request over a network easier than with `XMLHttpRequest`. It allows you to write your request using the "pipeline" style.

## Access

To access the fetch API, you need to use the following `open` declaration.

```fs
open Fable.PowerPack
open Fable.PowerPack.Fetch
```

## Basic

In the next snippet, we are getting the content of the `http://fable.io` page.

```fs
    fetch "http://fable.io" []
    |> Promise.bind (fun res -> res.text())
    |> Promise.map (fun txt ->
        // Access your resource here
        Browser.console.log txt
    )
```

We can write the same request using **Promise computation**.

```fs
    promise {
        let! res = fetch "http://fable.io" []
        let! txt = res.text()
        // Access your resource here
        Browser.console.log txt
    }
```

## Send a POST request with data

```fs
    open Fable.Core.JsInterop

    // This is the data we want to send to the server
    let data = {
        Username: "Maxime"
        Password: "myPassword"
    }

    let defaultProps =
        [ RequestProperties.Method HttpMethod.POST
        ; requestHeaders [ContentType "application/json"]
        ; RequestProperties.Body <| unbox(toJson data)]

    promise {
        let! res = fetch "http://my-server.com/sign-in" defaultProps
        let! txt = res.text()
        // Here you got access to the server response
        if txt = "true" then
            // Sign in: OK
        else
            // Sign in: Failed
    }
```

Fable PowerPack already implements some general helpers for you:

- postRecord: Sends a HTTP POST with the record serialized as JSON.
- tryPostRecord: Sends a HTTP POST with the record serialized as JSON and encapsulates it in a `Result` case.
- putRecord: Sends a HTTP PUT with the record serialized as JSON.
- etc.
