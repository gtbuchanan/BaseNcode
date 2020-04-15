namespace BaseNcode.FSharp

open System
open System.Text

[<AutoOpen>]
module internal Internals =
  let flip f x y = f y x

  let konst x = fun _ -> x

  let ifElse f g h x = if f x then g x else h x

  // Math
  let inline divRem a b = a / b, a % b

module internal Result =
  let errorWith f = function Ok ok -> ok | Error error -> f error

module internal String =
  let fromChars (chars: char[]) = new string(chars)

  let item i (s: string) = s.Chars i

  let padLeftWith totalLength paddingChar (s: string) = s.PadLeft (totalLength, paddingChar)

  let padRightWith totalLength paddingChar (s: string) = s.PadRight (totalLength, paddingChar)

  let replaceChar (oldChar: char) newChar (s: string) = s.Replace (oldChar, newChar)

  let trimEnd (trimChars: char seq) (s: string) = s.TrimEnd (Seq.toArray trimChars)

module internal StringBuilder =
  let toString (sb: StringBuilder) = sb.ToString ()

module internal Byte =
  let private toBinary (b: byte) = Convert.ToString (b, 2)

  let fromBinary b = Convert.ToByte (b, 2)

  let toOctet = toBinary >> String.padLeftWith 8 '0'

module internal Int =
  let toBinary (n: int) = Convert.ToString (n, 2)

  let fromBinary (s: string) = Convert.ToInt32 (s, 2)
