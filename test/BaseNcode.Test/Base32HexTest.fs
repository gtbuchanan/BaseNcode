module Base32HexTest

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
  getBytes "f", true, "CO======"
  getBytes "fo", true, "CPNG===="
  getBytes "foo", true, "CPNMU==="
  getBytes "foob", true, "CPNMUOG="
  getBytes "fooba", true, "CPNMUOJ1"
  getBytes "foobar", true, "CPNMUOJ1E8======"

  getBytes "f", false, "CO"
  getBytes "fo", false, "CPNG"
  getBytes "foo", false, "CPNMU"
  getBytes "foob", false, "CPNMUOG"
  getBytes "foobar", false, "CPNMUOJ1E8"
]

let decodeVectors = [
  "2ADWF5GI", Error [InvalidChar { Index = 3; Char = 'W' }]
  "XM2ERP6NY", Error [
    InvalidChar { Index = 8; Char = 'Y' };
    InvalidChar { Index = 0; Char = 'X' } ]
  "MODC6NTZ", Error [InvalidChar { Index = 7; Char = 'Z' }]
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
let ``Base32Hex.encode.should return encoded string`` (bytes, pad, s) =
  test <@ Base32Hex.encode pad bytes = s @>

[<Theory; ClassData(typeof<DecodeData>)>]
let ``Base32Hex.decode.should return decoded bytes`` (s, bytes) =
  test <@ Base32Hex.decode s = bytes @>

[<Property>]
let ``Base32Hex.Encode(seq<byte>, bool).should throw ArgumentNullException when bytes is null`` (pad) =
  raisesWith<ArgumentNullException> <@ Base32Hex.Encode (null, pad) @> <| fun e -> <@ e.ParamName = "bytes" @>

[<Theory; ClassData(typeof<EncodeData>)>]
let ``Base32Hex.Encode(seq<byte>, bool).should return encoded string`` (bytes, pad, s) =
  test <@ Base32Hex.Encode (bytes, pad) = s @>

[<Fact>]
let ``Base32Hex.Encode(seq<byte>).should throw ArgumentNullException when bytes is null`` () =
  raisesWith<ArgumentNullException> <@ Base32Hex.Encode (null) @> <| fun e -> <@ e.ParamName = "bytes" @>

[<Theory; ClassData(typeof<EncodePaddedData>)>]
let ``Base32Hex.Encode(seq<byte>).should return encoded string with padding`` (bytes, s) =
  test <@ Base32Hex.Encode (bytes) = s @>

[<Fact>]
let ``Base32Hex.Decode(string).should throw ArgumentNullException when s is null`` () =
  raisesWith<ArgumentNullException> <@ Base32Hex.Decode (null) @> <| fun e -> <@ e.ParamName = "s" @>

[<Theory; ClassData(typeof<DecodeFailureData>)>]
let ``Base32Hex.Decode(string).should throw ArgumentException when s is invalid`` (s) =
  raisesWith<ArgumentException> <@ Base32Hex.Decode s @> <| fun e -> <@ e.ParamName = "s" @>

[<Theory; ClassData(typeof<DecodeSuccessData>)>]
let ``Base32Hex.Decode(string).should return decoded bytes`` (s, bytes) =
  test <@ Base32Hex.Decode s = bytes @>
