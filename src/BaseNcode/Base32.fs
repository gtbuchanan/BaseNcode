namespace BaseNcode.FSharp

open System
open System.Text
open System.Diagnostics.CodeAnalysis

[<ExcludeFromCodeCoverage>]
type InvalidCharInfo = { Index: int; Char: char }

[<ExcludeFromCodeCoverage>]
type Base32DecodeError = InvalidChar of InvalidCharInfo

module Base32DecodeError =
  let toString = function
    | InvalidChar ic -> sprintf "Invalid character '%c' at index %i" ic.Char ic.Index

module Base32 =
  /// <summary>
  ///   Characters valid for use in the Base32 encoding. Numbers which users
  ///   might confuse with letters are omitted (i.e. 0, 1, 8, 9).
  /// </summary>
  let private base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"

  let encodeWithAlphabet alphabet pad =
    let toChar =
      String.fromChars
      >> String.padRightWith 5 '0'
      >> Int.fromBinary
      >> flip String.item alphabet

    let addPadding (s: string) =
      let octets = float s.Length / 8.0 |> ceil |> int
      s.PadRight (octets * 8, '=')

    Seq.collect Byte.toOctet
    >> Seq.chunkBySize 5
    >> Seq.map toChar
    >> String.Concat
    >> ifElse (konst pad) addPadding id

  let encode pad bytes = encodeWithAlphabet base32Alphabet pad bytes

  let private decodeTrimmed (alphabet: string) builder =
    let toBinary i c =
      match Char.ToUpperInvariant c |> alphabet.IndexOf with
      | -1 -> InvalidChar { Index = i; Char = c } |> Error
      | n -> Int.toBinary n |> String.padLeftWith 5 '0' |> Ok

    // Short-circuit rather than just ignore invalid chars
    // https://tools.ietf.org/html/rfc4648#section-12
    let foldBinary (acc: Result<StringBuilder, Base32DecodeError list>) (cur: Result<string, Base32DecodeError>) =
      match cur,acc with
      | Ok s, Ok sb -> sb.Append s |> Ok
      | Error e, Error errors -> e::errors |> Error
      | Error e, _ -> Error [e]
      | _, r -> r
    
    let decodeBinary =
      StringBuilder.toString
      >> Seq.chunkBySize 8
      >> Seq.takeWhile (Array.length >> (=) 8)
      >> Seq.map (String.fromChars >> Byte.fromBinary)
      >> Seq.toArray

    Seq.mapi toBinary
    >> Seq.fold foldBinary (Ok builder)
    >> Result.map decodeBinary

  let decodeWithAlphabet alphabet = String.trimEnd ['='] >> function
    | "" -> Ok Array.empty
    | s -> decodeTrimmed alphabet (StringBuilder()) s

  let decode s = decodeWithAlphabet base32Alphabet s

  let throwDecodeError arg =
    Seq.map Base32DecodeError.toString
    >> String.concat Environment.NewLine
    >> invalidArg arg

namespace BaseNcode

open BaseNcode.FSharp

[<AbstractClass; Sealed>]
type Base32 =
  static member Encode(bytes, pad) =
    if isNull bytes then nullArg (nameof bytes) else Base32.encode pad bytes

  static member Encode bytes = Base32.Encode (bytes, true)

  static member Decode s =
    if isNull s then nullArg (nameof s)
    else Base32.decode s |> Result.errorWith (Base32.throwDecodeError (nameof s))
