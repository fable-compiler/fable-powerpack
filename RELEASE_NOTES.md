### 1.3.6

* Add `andFor` operator for parallelization in promise CE (@panesofglass)

### 1.3.5

* Add Promise.mapResultError (@nojaf)
* Add Promise.tap (@johnsonw)

### 1.3.4

* Date.Local Russian localization (@irium)

### 1.3.3

* Update dependencies

### 1.3.2

* Add BrowserLocalStorage helpers
* Add Promise.catchEnd, eitherEnd and tryStart

### 1.3.1

* Fixes to indexedDB and promise test #33
* Make Error type in `Promise.bindResult` generic
* Add keyboard codes

### 1.3.0-beta-001

* Add `Date` module
* Improve tests readability

### 1.2.0

* Add `Promise.catchBind` - a variant of `catch` that takes a continuation returning `Promise<'T>`.
* **BREAKING CHANGE**: The signature of `Promise.either` has changed: both `success` and `fail` continuations are now required to return `U2<'T, Promise<'T>>` (which matches the ECMA spec).

### 1.1.2

* Add case to `HttpRequestHeaders` for custom headers

### 1.1.1

* Constraint return type of PromiseBuilder.ReturnFrom

### 1.0.0-beta-1

* Update to Fable 1.0

### 0.0.20

* Add `Promise.start`

### 0.0.19

* Add HTTP verb patch support #4

### 0.0.18

* Add `PromiseSeq` and experimental IndexedDB API

### 0.0.17

* Adding in tryFetch, tryFetchAs, and tryPostRecord (see #3)

### 0.0.16

* `fetch`, `fetchAs`, and `postRecord` now throw an exception on non-2xx HTTP status codes

* `fetch`, `fetchAs`, and `postRecord` now accept curried arguments rather than tupled arguments

### 0.0.15

* `Promise.catch` continuation passes an exception

### 0.0.14

* Compile using latest fable-compiler

### 0.0.12

* Add `Promise.Parallel`
* Add `umd` distribution
* Add `fetch` test

### 0.0.11

* Fix fetch error

### 0.0.10

* Remove peer dependencies

### 0.0.9

* Build with fable 0.7 alpha round 2

### 0.0.8

* Use only scripts for fable-powerpack

### 0.0.7

* Use relative path with `EntryModuleAttribute`

### 0.0.6

* Update fable-core version

### 0.0.5

* Add sourcemaps

### 0.0.4

* Import "fable-core/es2015" in ES2015 modules

### 0.0.3

* Include files compiled with ES2015 modules in the npm package

### 0.0.2

* Fix promise computation expression

### 0.0.1

* Initial release
