// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MvvmCross.Exceptions;

namespace MvvmCross.Base
{
#nullable enable
    public abstract class MvxParser
    {
        protected string? FullText { get; private set; }
        protected int CurrentIndex { get; private set; }

        protected virtual void Reset(string? textToParse)
        {
            FullText = textToParse;
            CurrentIndex = 0;
        }

        protected bool IsComplete
        {
            get
            {
                CheckFullTextValid();
                return CurrentIndex >= FullText!.Length;
            }
        }

        protected char CurrentChar
        {
            get
            {
                CheckFullTextValid();
                return FullText![CurrentIndex];
            }
        }

        protected string ReadQuotedString()
        {
            bool nextCharEscaped = false;
            char quoteDelimiterChar = CurrentChar;

            if (quoteDelimiterChar != '\'' && quoteDelimiterChar != '\"')
            {
                throw new MvxException(
                    $"Error parsing string indexer - unexpected quote character {quoteDelimiterChar} in text {FullText}");
            }

            MoveNext();
            if (IsComplete)
            {
                throw new MvxException($"Error parsing string indexer - unterminated in text {FullText}");
            }

            var textBuilder = new StringBuilder();
            while (true)
            {
                if (IsComplete)
                {
                    throw new MvxException($"Error parsing string indexer - unterminated in text {FullText}");
                }

                if (nextCharEscaped)
                {
                    textBuilder.Append(ReadEscapedCharacter());
                    nextCharEscaped = false;
                    continue;
                }

                var currentChar = CurrentChar;
                MoveNext();

                if (currentChar == '\\')
                {
                    nextCharEscaped = true;
                    continue;
                }

                if (currentChar == quoteDelimiterChar)
                {
                    break;
                }

                textBuilder.Append(currentChar);
            }

            return textBuilder.ToString();
        }

        protected uint ReadUnsignedInteger()
        {
            var integerStringBuilder = new StringBuilder();
            while (!IsComplete && char.IsDigit(CurrentChar))
            {
                integerStringBuilder.Append(CurrentChar);
                MoveNext();
            }
            uint index;
            var integerText = integerStringBuilder.ToString();
            if (!uint.TryParse(integerText, out index))
            {
                throw new MvxException($"Unable to parse integer text from {integerText} in {FullText}");
            }
            return index;
        }

        protected char ReadEscapedCharacter()
        {
            var currentChar = CurrentChar;
            MoveNext();

            // list here based on the very helpful
            // http://dotneteers.net/blogs/divedeeper/archive/2008/08/03/ParsingCSharpStrings.aspx
            switch (currentChar)
            {
                case 't':
                    return '\t';

                case 'r':
                    return '\r';

                case 'n':
                    return '\n';

                case '\'':
                    return '\'';

                case '\"':
                    return '\"';

                case '\\':
                    return '\\';

                case '0':
                    return '\0';

                case 'a':
                    return '\a';

                case 'b':
                    return '\b';

                case 'f':
                    return '\f';

                case 'v':
                    return '\v';

                case 'x':
                    // Hexa escape (1-4 digits)
                    // SL - decided not to support these as they are too ambiguous in length
                    //    - force users to use \u instead
                    throw new MvxException(
                        "We don't support string literals containing \\x - suggest using \\u escaped characters instead");
                case 'u':
                    // Unicode hexa escape (exactly 4 digits)
                    return ReadFourDigitUnicodeCharacter();

                case 'U':
                    // Unicode hexa escape (exactly 8 digits, first four must be 0000)
                    var firstFourDigits = ReadNDigits(4);
                    if (firstFourDigits != "0000")
                        throw new MvxException($"\\U unicode character does not start with 0000 in {FullText}");
                    return ReadFourDigitUnicodeCharacter();

                default:
                    throw new MvxException("Sorry we don't currently support escaped characters like \\{0}",
                                           currentChar);
            }
        }

        private char ReadFourDigitUnicodeCharacter()
        {
            var digits = ReadNDigits(4);
            var number = uint.Parse(digits, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);
            if (number > ushort.MaxValue)
                throw new MvxException($"\\u unicode character {number} out of range in {FullText}");
            return (char)number;
        }

        private string ReadNDigits(int count)
        {
            var toReturn = new StringBuilder(count);
            for (int i = 0; i < count; i++)
            {
                if (IsComplete)
                    throw new MvxException($"Error while reading {i + 1} of {count} digits in {FullText}");

                var currentChar = CurrentChar;
                if (!char.IsDigit(currentChar))
                    throw new MvxException($"Error while reading {i + 1} of {count} digits in {FullText} - not a char {currentChar}");

                toReturn.Append(currentChar);
                MoveNext();
            }

            return toReturn.ToString();
        }

        protected void MoveNext(uint increment = 1)
        {
            CurrentIndex += (int)increment;
        }

        protected void SkipWhitespaceAndCharacters(params char[] toSkip)
        {
            SkipWhitespaceAndCharacters((IEnumerable<char>)toSkip);
        }

        protected void SkipWhitespaceAndCharacters(IEnumerable<char> toSkip)
        {
            if (toSkip == null)
                throw new ArgumentNullException(nameof(toSkip));

            var skipChars = toSkip.ToArray();
            while (!IsComplete
                   && IsWhiteSpaceOrCharacter(CurrentChar, skipChars))
            {
                MoveNext();
            }
        }

        protected void SkipWhitespaceAndCharacters(Dictionary<char, bool> toSkip)
        {
            if (toSkip == null)
                throw new ArgumentNullException(nameof(toSkip));

            while (!IsComplete
                   && IsWhiteSpaceOrCharacter(CurrentChar, toSkip))
            {
                MoveNext();
            }
        }

        protected void SkipWhitespace()
        {
            while (!IsComplete
                   && char.IsWhiteSpace(CurrentChar))
            {
                MoveNext();
            }
        }

        private static bool IsWhiteSpaceOrCharacter(char charToTest, Dictionary<char, bool> toSkip)
        {
            return char.IsWhiteSpace(charToTest) || toSkip.ContainsKey(charToTest);
        }

        private static bool IsWhiteSpaceOrCharacter(char charToTest, IEnumerable<char> toSkip)
        {
            return char.IsWhiteSpace(charToTest) || toSkip.Contains(charToTest);
        }

        protected object? ReadValue()
        {
            if (!TryReadValue(AllowNonQuotedText.Allow, out var toReturn))
                throw new MvxException("Unable to read value");
            return toReturn;
        }

        protected enum AllowNonQuotedText
        {
            Allow,
            DoNotAllow
        }

        protected bool TryReadValue(AllowNonQuotedText allowNonQuotedText, out object? value)
        {
            SkipWhitespace();

            if (IsComplete)
            {
                throw new MvxException($"Unexpected termination while reading value in {FullText}");
            }

            var currentChar = CurrentChar;
            if (currentChar == '\'' || currentChar == '\"')
            {
                value = ReadQuotedString();
                return true;
            }

            if (char.IsDigit(currentChar) || currentChar == '-')
            {
                value = ReadNumber();
                return true;
            }

            bool booleanValue;
            if (TryReadBoolean(out booleanValue))
            {
                value = booleanValue;
                return true;
            }

            if (TryReadNull())
            {
                value = null;
                return true;
            }

            if (allowNonQuotedText == AllowNonQuotedText.Allow)
            {
                value = ReadTextUntil(',', ';');
                return true;
            }

            value = null;
            return false;
        }

        protected bool TestKeywordInPeekString(string uppercaseKeyword, string peekString)
        {
            if (peekString == null)
                return false;

            if (uppercaseKeyword == null)
                return false;

            if (peekString.Length < uppercaseKeyword.Length)
                return false;

            if (peekString.Length != uppercaseKeyword.Length
                && IsValidMidCharacterOfCSharpName(peekString[uppercaseKeyword.Length]))
                return false;

            if (!peekString.StartsWith(uppercaseKeyword, StringComparison.Ordinal))
                return false;

            return true;
        }

        protected bool TryReadNull()
        {
            var peek = SafePeekString(5);
            peek = peek.ToUpperInvariant();
            if (TestKeywordInPeekString("NULL", peek))
            {
                MoveNext(4);
                return true;
            }

            return false;
        }

        protected bool TryReadBoolean(out bool booleanValue)
        {
            var peek = SafePeekString(6);
            peek = peek.ToUpperInvariant();
            if (TestKeywordInPeekString("TRUE", peek))
            {
                MoveNext(4);
                booleanValue = true;
                return true;
            }
            if (TestKeywordInPeekString("FALSE", peek))
            {
                MoveNext(5);
                booleanValue = false;
                return true;
            }

            booleanValue = false;
            return false;
        }

        protected string SafePeekString(int length)
        {
            var safeLength = Math.Min(length, (FullText?.Length ?? 0) - CurrentIndex);
            if (safeLength <= 0)
                return string.Empty;
            return FullText!.Substring(CurrentIndex, safeLength);
        }

        protected ValueType ReadNumber()
        {
            var stringBuilder = new StringBuilder();

            var firstChar = CurrentChar;
            if (firstChar == '-')
            {
                stringBuilder.Append(firstChar);
                MoveNext();
            }

            var decimalPeriodSeen = false;

            while (!IsComplete)
            {
                var currentChar = CurrentChar;
                // note that we force users to use . as the decimal separator (no European commas allowed)
                if (currentChar == '.')
                {
                    if (decimalPeriodSeen)
                        throw new MvxException($"Multiple decimal places seen in number in {FullText} at position {CurrentIndex}");
                    decimalPeriodSeen = true;
                }
                else if (!char.IsDigit(currentChar))
                {
                    break;
                }

                stringBuilder.Append(currentChar);
                MoveNext();
            }

            var numberText = stringBuilder.ToString();
            return NumberFromText(numberText, decimalPeriodSeen);
        }

        protected ValueType NumberFromText(string numberText)
        {
            if (numberText == null)
                throw new ArgumentNullException(nameof(numberText));

            return NumberFromText(numberText, numberText.Contains("."));
        }

        protected ValueType NumberFromText(string numberText, bool decimalPeriodSeen)
        {
            if (decimalPeriodSeen)
            {
                double doubleResult;
                if (double.TryParse(numberText,
                                    NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture,
                                    out doubleResult))
                    return doubleResult;

                throw new MvxException($"Failed to parse double from {numberText} in {FullText}");
            }
            else
            {
                // note that we use Int64 because Json.Net doe...
                long intResult;
                if (long.TryParse(numberText,
                                   NumberStyles.AllowLeadingSign,
                                   CultureInfo.InvariantCulture,
                                   out intResult))
                    return intResult;

                throw new MvxException($"Failed to parse Int64 from {numberText} in {FullText}");
            }
        }

        protected object ReadEnumerationValue(Type enumerationType, bool ignoreCase = true)
        {
            if (enumerationType == null)
                throw new ArgumentNullException(nameof(enumerationType));

            var name = ReadValidCSharpName();
            try
            {
                return Enum.Parse(enumerationType, name, ignoreCase);
            }
            catch (ArgumentException exception)
            {
                throw exception.MvxWrap($"Problem parsing {enumerationType.Name} from {name} in {FullText}");
            }
        }

        protected string ReadTextUntilWhitespaceOr(params char[] terminatingCharacters)
        {
            var toReturn = new StringBuilder();

            while (!IsComplete)
            {
                var currentChar = CurrentChar;
                if (terminatingCharacters.Contains(currentChar)
                    || char.IsWhiteSpace(currentChar))
                {
                    break;
                }
                toReturn.Append(currentChar);
                MoveNext();
            }

            return toReturn.ToString();
        }

        protected string ReadTextUntil(params char[] terminatingCharacters)
        {
            var toReturn = new StringBuilder();

            while (!IsComplete)
            {
                var currentChar = CurrentChar;
                if (terminatingCharacters.Contains(currentChar))
                {
                    break;
                }
                toReturn.Append(currentChar);
                MoveNext();
            }

            return toReturn.ToString();
        }

        protected string ReadValidCSharpName()
        {
            SkipWhitespace();
            var firstChar = CurrentChar;
            if (!IsValidFirstCharacterOfCSharpName(firstChar))
            {
                throw new MvxException($"PropertyName must start with letter - position {CurrentIndex} in {FullText} - char {firstChar}");
            }
            var toReturn = new StringBuilder();
            toReturn.Append(firstChar);
            MoveNext();
            while (!IsComplete)
            {
                var currentChar = CurrentChar;
                if (!char.IsLetterOrDigit(currentChar)
                    && currentChar != '_')
                {
                    break;
                }
                toReturn.Append(currentChar);
                MoveNext();
            }
            return toReturn.ToString();
        }

        protected bool IsValidFirstCharacterOfCSharpName(char firstChar)
        {
            return char.IsLetter(firstChar) || firstChar == '_';
        }

        protected bool IsValidMidCharacterOfCSharpName(char firstChar)
        {
            return char.IsLetterOrDigit(firstChar) || firstChar == '_';
        }

        private void CheckFullTextValid()
        {
            if (FullText == null)
            {
                throw new InvalidOperationException("Please call Reset first");
            }
        }
    }
#nullable restore
}
