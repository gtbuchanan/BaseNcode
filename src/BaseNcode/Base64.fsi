namespace BaseNcode.FSharp

/// Represents an error encountered during decoding.
type Base64DecodeError =
  /// The input was in an invalid format.
  InvalidFormat of string

module internal Base64DecodeError =
  val toString : Base64DecodeError -> string

/// <summary>
///   Basic operations for the base64 encoding.
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc4648#section-4">
///   RFC 4648 Section 4: Base 64 Encoding
/// </seealso>
[<RequireQualifiedAccess>]
module Base64 =
  /// <summary>
  ///   Converts a sequence of bytes to a base64-encoded string.
  /// </summary>
  /// <param name="pad">
  ///   Specifies if the output should be padded. Pass <c>true</c> unless
  ///   the specification referring to RFC 4648 explicitly states otherwise.
  /// </param>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <returns>The sequence of bytes as a Base64-encoded string.</returns>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-4">
  ///   RFC 4648 Section 4: Base 64 Encoding
  /// </seealso>
  val encode : pad:bool -> bytes:seq<byte> -> string

  /// <summary>
  ///   Decodes a base64-encoded string into a sequence of bytes.
  /// </summary>
  /// <param name="s">The Base64 string to decode.</param>
  /// <returns>The decoded sequence of bytes if successful, otherwise the error.</returns>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-4">
  ///   RFC 4648 Section 4: Base 64 Encoding
  /// </seealso>
  val decode : s:string -> Result<byte[], Base64DecodeError>

namespace BaseNcode

/// <summary>
///   Basic operations for the base64 encoding.
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc4648#section-4">
///   RFC 4648 Section 4: Base 64 Encoding
/// </seealso>
[<AbstractClass; Sealed>]
type Base64 =
  /// <summary>
  ///   Converts a sequence of bytes to a base64-encoded string.
  /// </summary>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <param name="pad">
  ///   Specifies if the output should be padded. Pass <c>true</c> unless
  ///   the specification referring to RFC 4648 explicitly states otherwise.
  /// </param>
  /// <returns>The sequence of bytes as a base64-encoded string.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="bytes" /> is <c>null</c>.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-4">
  ///   RFC 4648 Section 4: Base 64 Encoding
  /// </seealso>
  static member Encode : bytes:seq<byte> * pad:bool -> string

  /// <summary>
  ///   Converts a sequence of bytes to a base64-encoded string with padding.
  /// </summary>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <returns>The sequence of bytes as a base64-encoded string.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="bytes" /> is <c>null</c>.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-4">
  ///   RFC 4648 Section 4: Base 64 Encoding
  /// </seealso>
  static member Encode : bytes:seq<byte> -> string

  /// <summary>
  ///   Decodes a base64-encoded string into a sequence of bytes.
  /// </summary>
  /// <param name="s">The base64 string to decode.</param>
  /// <returns>The decoded sequence of bytes if successful, otherwise the error.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
  /// <exception cref="ArgumentException"><paramref name="s" /> is in an invalid format.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-4">
  ///   RFC 4648 Section 4: Base 64 Encoding
  /// </seealso>
  static member Decode : s:string -> byte[]
