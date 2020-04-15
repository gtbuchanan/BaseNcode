namespace BaseNcode.FSharp

open System
open System.Diagnostics.CodeAnalysis

[<ExcludeFromCodeCoverage>]
type Base64DecodeError = InvalidFormat of string

module Base64DecodeError =
  let toString = function InvalidFormat m -> m

module Base64 =
  let encode pad bytes =
    Seq.toArray bytes
    |> Convert.ToBase64String
    |> ifElse (konst pad) id (String.trimEnd ['='])

  let decode (s: string) =
    try
      String.padRightWith (s.Length + s.Length * 3 % 4) '=' s
      |> Convert.FromBase64String |> Ok
    with
    | :? FormatException as fe -> InvalidFormat fe.Message |> Error

namespace BaseNcode

open BaseNcode.FSharp

[<AbstractClass; Sealed>]
type Base64 =
  static member Encode(bytes, pad) =
    if isNull bytes then nullArg (nameof bytes) else Base64.encode pad bytes

  static member Encode(bytes) = Base64.Encode (bytes, true)

  static member Decode(s) =
    if isNull s then nullArg (nameof s)
    else Base64.decode s |> Result.errorWith (Base64DecodeError.toString >> invalidArg (nameof s))
