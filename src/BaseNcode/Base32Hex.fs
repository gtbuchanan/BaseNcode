namespace BaseNcode.FSharp

module Base32Hex =
  let private alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUV"

  let encode pad bytes = Base32.encodeWithAlphabet alphabet pad bytes

  let decode s = Base32.decodeWithAlphabet alphabet s

namespace BaseNcode

open BaseNcode.FSharp

[<AbstractClass; Sealed>]
type Base32Hex =
  static member Encode(bytes, pad) =
    if isNull bytes then nullArg (nameof bytes) else Base32Hex.encode pad bytes

  static member Encode(bytes) = Base32Hex.Encode (bytes, true)

  static member Decode(s) =
    if isNull s then nullArg (nameof s)
    else Base32Hex.decode s |> Result.errorWith (Base32.throwDecodeError (nameof s))
