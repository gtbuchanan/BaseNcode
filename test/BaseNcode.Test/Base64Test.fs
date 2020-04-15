module Base64Test

open Xunit
open FsCheck.Xunit
open Swensen.Unquote

open System
open System.Text

open BaseNcode
open BaseNcode.FSharp

let getBytes (s: string) = Encoding.UTF8.GetBytes s

let allVectors = [
  // RFC4648 Test Vectors (https://tools.ietf.org/html/rfc4648#section-10)
  Array.empty, true, ""
  getBytes "f", true, "Zg=="
  getBytes "fo", true, "Zm8="
  getBytes "foo", true, "Zm9v"
  getBytes "foob", true, "Zm9vYg=="
  getBytes "fooba", true, "Zm9vYmE="
  getBytes "foobar", true, "Zm9vYmFy"

  getBytes "f", false, "Zg"
  getBytes "fo", false, "Zm8"
  getBytes "foob", false, "Zm9vYg"
  getBytes "fooba", false, "Zm9vYmE"
]

type EncodeData() as this =
  inherit TheoryData<byte[], bool, string>()
  do Seq.iter this.Add allVectors

type EncodePaddedData() as this =
  inherit TheoryData<byte[], string>()
  do
     Seq.choose (fun (bytes, pad, s) -> if pad then Some (bytes, s) else None) allVectors
     |> Seq.iter this.Add

type DecodeData() as this =
  inherit TheoryData<string, Result<byte[], Base64DecodeError>>()
  do
    for bytes, _, s in allVectors do this.Add (s, Ok bytes)

type DecodeSuccessData() as this =
  inherit TheoryData<string, byte[]>()
  do
    for bytes, _, s in allVectors do this.Add (s, bytes)

[<Theory; ClassData(typeof<EncodeData>)>]
let ``Base64.encode.should return encoded string`` (bytes, pad, s) =
  test <@ Base64.encode pad bytes = s @>

[<Theory; ClassData(typeof<DecodeData>)>]
let ``Base64.decode.should return decoded bytes`` (s, bytes) =
  test <@ Base64.decode s = bytes @>

[<Property>]
let ``Base64.Encode(seq<byte>, bool).should throw ArgumentNullException when bytes is null`` (pad) =
  raisesWith<ArgumentNullException> <@ Base64.Encode (null, pad) @> <| fun e -> <@ e.ParamName = "bytes" @>

[<Theory; ClassData(typeof<EncodeData>)>]
let ``Base64.Encode(seq<byte>, bool).should return encoded string`` (bytes, pad, s) =
  test <@ Base64.Encode (bytes, pad) = s @>

[<Fact>]
let ``Base64.Encode(seq<byte>).should throw ArgumentNullException when bytes is null`` () =
  raisesWith<ArgumentNullException> <@ Base64.Encode (null) @> <| fun e -> <@ e.ParamName = "bytes" @>

[<Theory; ClassData(typeof<EncodePaddedData>)>]
let ``Base64.Encode(seq<byte>).should return encoded string with padding`` (bytes, s) =
  test <@ Base64.Encode (bytes) = s @>

[<Fact>]
let ``Base64.Decode(string).should throw ArgumentNullException when s is null`` () =
  raisesWith<ArgumentNullException> <@ Base64.Decode (null) @> <| fun e -> <@ e.ParamName = "s" @>

[<Fact>]
let ``Base64.Decode(string).should throw ArgumentException when s is invalid`` () =
  raisesWith<ArgumentException> <@ Base64.Decode ("Zm9:vYmFy~") @> <| fun e -> <@ e.ParamName = "s" @>

[<Theory; ClassData(typeof<DecodeSuccessData>)>]
let ``Base64.Decode(string).should return decoded bytes`` (s, bytes) =
  test <@ Base64.Decode s = bytes @>
