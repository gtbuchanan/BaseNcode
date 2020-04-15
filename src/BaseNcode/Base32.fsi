namespace BaseNcode.FSharp

/// Represents error info for an invalid character encountered during decoding.
type InvalidCharInfo = {
  /// The index of the invalid character.
  Index: int;
  /// The invalid character.
  Char: char }

/// Represents an error encountered during decoding.
type Base32DecodeError =
  /// An invalid character was encountered.
  InvalidChar of InvalidCharInfo

[<RequireQualifiedAccess>]
module internal Base32DecodeError =
  val toString : Base32DecodeError -> string

/// <summary>
///   Basic operations for the Base32 encoding.
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc4648#section-6">
///   RFC 4648 Section 6: Base 32 Encoding
/// </seealso>
[<RequireQualifiedAccess>]
module Base32 =
  val internal encodeWithAlphabet : alphabet:string -> pad:bool -> (seq<byte> -> string)

  /// <summary>
  ///   Converts a sequence of bytes to a Base32-encoded string.
  /// </summary>
  /// <param name="pad">
  ///   Specifies if the output should be padded. Pass <c>true</c> unless
  ///   the specification referring to RFC 4648 explicitly states otherwise.
  /// </param>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <returns>The sequence of bytes as a Base32-encoded string.</returns>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-6">
  ///   RFC 4648 Section 6: Base 32 Encoding
  /// </seealso>
  val encode : pad:bool -> bytes:seq<byte> -> string

  val internal decodeWithAlphabet : alphabet:string -> (string -> Result<byte[], Base32DecodeError list>)

  /// <summary>
  ///   Decodes a Base32-encoded string into a sequence of bytes.
  /// </summary>
  /// <param name="s">The Base32 string to decode.</param>
  /// <returns>The decoded sequence of bytes if successful, otherwise a list of errors.</returns>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-6">
  ///   RFC 4648 Section 6: Base 32 Encoding
  /// </seealso>
  val decode : s:string -> Result<byte[], Base32DecodeError list>

  val internal throwDecodeError : arg:string -> (seq<Base32DecodeError> -> 'a)

namespace BaseNcode

/// <summary>
///   Basic operations for the base32 encoding.
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc4648#section-6">
///   RFC 4648 Section 6: Base 32 Encoding
/// </seealso>
[<AbstractClass; Sealed>]
type Base32 =
  /// <summary>
  ///   Converts a sequence of bytes to a base32-encoded string.
  /// </summary>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <param name="pad">
  ///   Specifies if the output should be padded. Pass <c>true</c> unless
  ///   the specification referring to RFC 4648 explicitly states otherwise.
  /// </param>
  /// <returns>The sequence of bytes as a base32-encoded string.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="bytes" /> is <c>null</c>.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-6">
  ///   RFC 4648 Section 6: Base 32 Encoding
  /// </seealso>
  static member Encode : bytes:seq<byte> * pad:bool -> string

  /// <summary>
  ///   Converts a sequence of bytes to a base32-encoded string with padding.
  /// </summary>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <returns>The sequence of bytes as a base32-encoded string.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="bytes" /> is <c>null</c>.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-6">
  ///   RFC 4648 Section 6: Base 32 Encoding
  /// </seealso>
  static member Encode : bytes:seq<byte> -> string

  /// <summary>
  ///   Decodes a base32-encoded string into a sequence of bytes.
  /// </summary>
  /// <param name="s">The base32 string to decode.</param>
  /// <returns>The decoded sequence of bytes.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="s" /> is <c>null</c>.</exception>
  /// <exception cref="ArgumentException"><paramref cref="s" /> contains invalid characters.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-6">
  ///   RFC 4648 Section 6: Base 32 Encoding
  /// </seealso>
  static member Decode : s:string -> byte[]
