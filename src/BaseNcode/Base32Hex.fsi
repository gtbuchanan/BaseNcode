namespace BaseNcode.FSharp

/// <summary>
///   Basic operations for the base32hex encoding.
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc4648#section-7">
///   RFC 4648 Section 7: Base 32 Encoding with Extended Hex Alphabet
/// </seealso>
[<RequireQualifiedAccess>]
module Base32Hex =
  /// <summary>
  ///   Converts a sequence of bytes to a base32hex-encoded string.
  /// </summary>
  /// <param name="pad">
  ///   Specifies if the output should be padded. Pass <c>true</c> unless
  ///   the specification referring to RFC 4648 explicitly states otherwise.
  /// </param>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <returns>The sequence of bytes as a Base32-encoded string.</returns>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-7">
  ///   RFC 4648 Section 7: Base 32 Encoding with Extended Hex Alphabet
  /// </seealso>
  val encode : pad:bool -> bytes:seq<byte> -> string

  /// <summary>
  ///   Decodes a base32hex-encoded string into a sequence of bytes.
  /// </summary>
  /// <param name="str">The base32hex string to decode.</param>
  /// <returns>The decoded sequence of bytes if successful, otherwise a list of errors.</returns>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-7">
  ///   RFC 4648 Section 7: Base 32 Encoding with Extended Hex Alphabet
  /// </seealso>
  val decode : s:string -> Result<byte[], Base32DecodeError list>

namespace BaseNcode

/// <summary>
///   Basic operations for the base32hex encoding.
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc4648#section-7">
///   RFC 4648 Section 7: Base 32 Encoding with Extended Hex Alphabet
/// </seealso>
[<AbstractClass; Sealed>]
type Base32Hex =
  /// <summary>
  ///   Converts a sequence of bytes to a base32hex-encoded string.
  /// </summary>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <param name="pad">
  ///   Specifies if the output should be padded. Pass <c>true</c> unless
  ///   the specification referring to RFC 4648 explicitly states otherwise.
  /// </param>
  /// <returns>The sequence of bytes as a base32hex-encoded string.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="bytes" /> is <c>null</c>.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-7">
  ///   RFC 4648 Section 7: Base 32 Encoding with Extended Hex Alphabet
  /// </seealso>
  static member Encode : bytes:seq<byte> * pad:bool -> string

  /// <summary>
  ///   Converts a sequence of bytes to a base32hex-encoded string with padding.
  /// </summary>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <returns>The sequence of bytes as a base32hex-encoded string.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="bytes" /> is <c>null</c>.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-7">
  ///   RFC 4648 Section 7: Base 32 Encoding with Extended Hex Alphabet
  /// </seealso>
  static member Encode : bytes:seq<byte> -> string

  /// <summary>
  ///   Decodes a base32hex-encoded string into a sequence of bytes.
  /// </summary>
  /// <param name="str">The base32hex string to decode.</param>
  /// <returns>The decoded sequence of bytes.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="s" /> is <c>null</c>.</exception>
  /// <exception cref="ArgumentException"><paramref cref="s" /> contains invalid characters.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-7">
  ///   RFC 4648 Section 7: Base 32 Encoding with Extended Hex Alphabet
  /// </seealso>
  static member Decode : s:string -> byte[]
