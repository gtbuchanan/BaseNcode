# BaseNcode

![CI](https://github.com/gtbuchanan/BaseNcode/workflows/CI/badge.svg)

A .NET library to encode data in Base-N. F# and C# friendly.

## Disclaimer

This library was created as an F# learning exercise. For a more performance-focused implementation, you may want to consider [SimpleBase](https://github.com/ssg/SimpleBase).

## Supported Encodings

* [base32](https://tools.ietf.org/html/rfc4648#section-6): `Base32` module
* [base32hex](https://tools.ietf.org/html/rfc4648#section-7): `Base32Hex` module
* [base64](https://tools.ietf.org/html/rfc4648#section-4): `Base64` module
* [base64url](https://tools.ietf.org/html/rfc4648#section-5): `Base64Url` module

## Usage

All encodings follow the same API pattern so you can extrapolate the following examples to any of the aforementioned encodings.

### Import

```fsharp
// F#
open BaseNcode.FSharp
```

```csharp
// C#
using BaseNcode;
```

### F# Example

```fsharp
open System.Text

// Encode
let bytes = Encoding.UTF8.GetBytes("foobar")
let encodedString = Base32.encode true bytes
printfn encodedString // MZXW6YTBOI======

// Decode
let decodeErrorToString = function
  | InvalidChar ic -> sprintf "Invalid character %c at index %i" ic.Char ic.Index

match Base32.decode encodedString with
| Ok decodedBytes -> Encoding.UTF8.GetString(decodedBytes) |> printfn // foobar
| Error errors -> Seq.map decodeErrorToString errors |> printfn
```

### C# Example

```csharp
using System.Text;

// Encode
var bytes = Encoding.UTF8.GetBytes("foobar");
var encodedString = Base32.Encode(bytes);
Console.WriteLine(encodedString); // MZXW6YTBOI======

// Decode
var decodedBytes = Base32.Decode(encodedString);
Console.WriteLine(Encoding.UTF8.GetString(decodedBytes)); // foobar
```
