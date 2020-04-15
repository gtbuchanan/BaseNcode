module Base64UrlTest

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

  // RFC7515 Test Vector (https://tools.ietf.org/html/rfc7515#appendix-C)
  [| 3uy; 236uy; 255uy; 224uy; 193uy |], false, "A-z_4ME"
]

type EncodeData() as this =
  inherit TheoryData<byte[], bool, string>()
  do Seq.iter this.Add allVectors

type EncodeNotPaddedData() as this =
  inherit TheoryData<byte[], string>()
  do
     Seq.choose (fun (bytes, pad, s) -> if pad then None else Some (bytes, s)) allVectors
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
let ``Base64Url.encode.should return encoded string`` (bytes, pad, s) =
  test <@ Base64Url.encode pad bytes = s @>

[<Theory; ClassData(typeof<DecodeData>)>]
let ``Base64Url.decode.should return decoded bytes`` (s, bytes) =
  test <@ Base64Url.decode s = bytes @>

[<Property>]
let ``Base64Url.Encode(seq<byte>, bool).should throw ArgumentNullException when bytes is null`` (pad) =
  raisesWith<ArgumentNullException> <@ Base64Url.Encode (null, pad) @> <| fun e -> <@ e.ParamName = "bytes" @>

[<Theory; ClassData(typeof<EncodeData>)>]
let ``Base64Url.Encode(seq<byte>, bool).should return encoded string`` (bytes, pad, s) =
  test <@ Base64Url.Encode (bytes, pad) = s @>

[<Fact>]
let ``Base64Url.Encode(seq<byte>).should throw ArgumentNullException when bytes is null`` () =
  raisesWith<ArgumentNullException> <@ Base64Url.Encode (null) @> <| fun e -> <@ e.ParamName = "bytes" @>

[<Theory; ClassData(typeof<EncodeNotPaddedData>)>]
let ``Base64Url.Encode(seq<byte>).should return encoded string without padding`` (bytes, s) =
  test <@ Base64Url.Encode (bytes) = s @>

[<Fact>]
let ``Base64Url.Decode(string).should throw ArgumentNullException when s is null`` () =
  raisesWith<ArgumentNullException> <@ Base64Url.Decode (null) @> <| fun e -> <@ e.ParamName = "s" @>

[<Fact>]
let ``Base64Url.Decode(string).should throw ArgumentException when s is invalid`` () =
  raisesWith<ArgumentException> <@ Base64Url.Decode ("Zm9:vYmFy~") @> <| fun e -> <@ e.ParamName = "s" @>

[<Theory; ClassData(typeof<DecodeSuccessData>)>]
let ``Base64Url.Decode(string).should return decoded bytes`` (s, bytes) =
  test <@ Base64Url.Decode s = bytes @>
