namespace BaseNcode.FSharp

/// <summary>
///   Basic operations for the base64url encoding.
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc4648#section-5">
///   RFC 4648 Section 5: Base 64 Encoding with URL and Filename Safe Alphabet
/// </seealso>
[<RequireQualifiedAccess>]
module Base64Url =
  /// <summary>
  ///   Converts a sequence of bytes to a base64url-encoded string.
  /// </summary>
  /// <param name="pad">
  ///   Specifies if the output should be padded. It's common to
  ///   to pass <c>false</c> for base64url to avoid needing to URL
  ///   encode the padding characters.
  /// </param>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <returns>The sequence of bytes as a base64url-encoded string.</returns>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-5">
  ///   RFC 4648 Section 5: Base 64 Encoding with URL and Filename Safe Alphabet
  /// </seealso>
  val encode : pad:bool -> bytes:seq<byte> -> string

  /// <summary>
  ///   Decodes a base64url-encoded string into a sequence of bytes.
  /// </summary>
  /// <param name="s">The base64url string to decode.</param>
  /// <returns>The decoded sequence of bytes if successful, otherwise the error.</returns>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-5">
  ///   RFC 4648 Section 5: Base 64 Encoding with URL and Filename Safe Alphabet
  /// </seealso>
  val decode : s:string -> Result<byte[], Base64DecodeError>

namespace BaseNcode

/// <summary>
///   Basic operations for the base64url encoding.
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc4648#section-5">
///   RFC 4648 Section 5: Base 64 Encoding with URL and Filename Safe Alphabet
/// </seealso>
/// <seealso href="https://tools.ietf.org/html/rfc7515#appendix-C">
///   RFC 7517 Appendix C: Notes on Implementing base64url Encoding without Padding
/// </seealso>
[<AbstractClass; Sealed>]
type Base64Url =
  /// <summary>
  ///   Converts a sequence of bytes to a base64url-encoded string.
  /// </summary>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <param name="pad">
  ///   Specifies if the output should be padded. It's common to
  ///   to pass <c>false</c> for base64url to avoid needing to URL
  ///   encode the padding characters.
  /// </param>
  /// <returns>The sequence of bytes as a base64url-encoded string.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="bytes" /> is <c>null</c>.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-5">
  ///   RFC 4648 Section 5: Base 64 Encoding with URL and Filename Safe Alphabet
  /// </seealso>
  static member Encode : bytes:seq<byte> * pad:bool -> string

  /// <summary>
  ///   Converts a sequence of bytes to a base64url-encoded string without padding.
  /// </summary>
  /// <param name="bytes">The sequence of bytes.</param>
  /// <returns>The sequence of bytes as a base64url-encoded string.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="bytes" /> is <c>null</c>.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-5">
  ///   RFC 4648 Section 5: Base 64 Encoding with URL and Filename Safe Alphabet
  /// </seealso>
  static member Encode : bytes:seq<byte> -> string

  /// <summary>
  ///   Decodes a base64url-encoded string into a sequence of bytes.
  /// </summary>
  /// <param name="s">The base64url string to decode.</param>
  /// <returns>The decoded sequence of bytes if successful, otherwise the error.</returns>
  /// <exception cref="ArgumentNullException><paramref cref="s" /> is <c>null</c>.</exception>
  /// <exception cref="ArgumentException"><paramref cref="s" /> is in an invalid format.</exception>
  /// <seealso href="https://tools.ietf.org/html/rfc4648#section-5">
  ///   RFC 4648 Section 5: Base 64 Encoding with URL and Filename Safe Alphabet
  /// </seealso>
  static member Decode : s:string -> byte[]
