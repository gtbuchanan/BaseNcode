module Base32Test

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
  getBytes "f", true, "MY======"
  getBytes "fo", true, "MZXQ===="
  getBytes "foo", true, "MZXW6==="
  getBytes "foob", true, "MZXW6YQ="
  getBytes "fooba", true, "MZXW6YTB"
  getBytes "foobar", true, "MZXW6YTBOI======"
  getBytes "f", false, "MY"
  getBytes "fo", false, "MZXQ"
  getBytes "foo", false, "MZXW6"
  getBytes "foob", false, "MZXW6YQ"
  getBytes "foobar", false, "MZXW6YTBOI"

  // Other valid vectors (https://cryptii.com/pipes/base32)
  [| 0xd0uy; 0x06uy; 0x5euy; 0x99uy |], true, "2ADF5GI="
  [| 0x3buy; 0x35uy; 0x98uy; 0xbfuy; 0xd8uy; 0xcfuy; 0x35uy |], true, "HM2ZRP6YZ42Q===="
]

let decodeVectors = [
  "2AD1F5GI", Error [InvalidChar { Index = 3; Char = '1' }]
  "8M2ZRP6Y0", Error [
    InvalidChar { Index = 8; Char = '0' };
    InvalidChar { Index = 0; Char = '8' } ]
  "MZXW6YT9", Error [InvalidChar { Index = 7; Char = '9' }]
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
  inherit TheoryData<string, Result<byte[], Base32DecodeError list>>()
  do
    for bytes, _, s in allVectors do this.Add (s, Ok bytes)
    Seq.iter this.Add decodeVectors

type DecodeSuccessData() as this =
  inherit TheoryData<string, byte[]>()
  do
    for bytes, _, s in allVectors do this.Add (s, bytes)

type DecodeFailureData() as this =
  inherit TheoryData<string>()
  do
    for s, _ in decodeVectors do this.Add s

[<Theory; ClassData(typeof<EncodeData>)>]
let ``Base32.encode.should return encoded string`` (bytes, pad, s) =
  test <@ Base32.encode pad bytes = s @>

[<Theory; ClassData(typeof<DecodeData>)>]
let ``Base32.decode.should return decoded bytes`` (s, bytes) =
  test <@ Base32.decode s = bytes @>

[<Property>]
let ``Base32.Encode(seq<byte>, bool).should throw ArgumentNullException when bytes is null`` (pad) =
  raisesWith<ArgumentNullException> <@ Base32.Encode (null, pad) @> <| fun e -> <@ e.ParamName = "bytes" @>

[<Theory; ClassData(typeof<EncodeData>)>]
let ``Base32.Encode(seq<byte>, bool).should return encoded string`` (bytes, pad, s) =
  test <@ Base32.Encode (bytes, pad) = s @>

[<Fact>]
let ``Base32.Encode(seq<byte>).should throw ArgumentNullException when bytes is null`` () =
  raisesWith<ArgumentNullException> <@ Base32.Encode (null) @> <| fun e -> <@ e.ParamName = "bytes" @>

[<Theory; ClassData(typeof<EncodePaddedData>)>]
let ``Base32.Encode(seq<byte>).should return encoded string with padding`` (bytes, s) =
  test <@ Base32.Encode (bytes) = s @>

[<Fact>]
let ``Base32.Decode(string).should throw ArgumentNullException when s is null`` () =
  raisesWith<ArgumentNullException> <@ Base32.Decode (null) @> <| fun e -> <@ e.ParamName = "s" @>

[<Theory; ClassData(typeof<DecodeSuccessData>)>]
let ``Base32.Decode(string).should return decoded bytes`` (s, bytes) =
  test <@ Base32.Decode s = bytes @>

[<Theory; ClassData(typeof<DecodeFailureData>)>]
let ``Base32.Decode(string).should throw ArgumentException when s is invalid`` (s) =
  raisesWith<ArgumentException> <@ Base32.Decode s @> <| fun e -> <@ e.ParamName = "s" @>
