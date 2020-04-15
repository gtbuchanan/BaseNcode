namespace BaseNcode.FSharp

module Base64Url =
  let encode pad bytes =
    Base64.encode pad bytes
    |> String.replaceChar '+' '-'
    |> String.replaceChar '/' '_'

  let decode (s: string) =
    String.replaceChar '_' '/' s
    |> String.replaceChar '-' '+'
    |> Base64.decode

namespace BaseNcode

open BaseNcode.FSharp

[<AbstractClass; Sealed>]
type Base64Url =
  static member Encode(bytes, pad) =
    if isNull bytes then nullArg (nameof bytes) else Base64Url.encode pad bytes

  static member Encode(bytes) = Base64Url.Encode (bytes, false)

  static member Decode(s) =
    if isNull s then nullArg (nameof s)
    else Base64Url.decode s |> Result.errorWith (Base64DecodeError.toString >> invalidArg (nameof s))
