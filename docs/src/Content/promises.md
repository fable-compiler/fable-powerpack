# Promise

## Basic promise

To build a promise you can use **Promise computation** which allow you a clean syntax to build them.

Example, here is a promise which return the result of `x + y` after `500ms` of delay

```fs
let add x y =
    promise {
        do! Promise.sleep 500 // Sleep for 500ms
        return x + y
    }
    |> Promise.start
```
