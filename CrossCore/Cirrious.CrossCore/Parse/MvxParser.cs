// MvxParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.CrossCore.Parse
{
    public abstract class MvxParser
    {
        protected string FullText { get; private set; }
        protected int CurrentIndex { get; private set; }

        protected virtual void Reset(string textToParse)
        {
            FullText = textToParse;
            CurrentIndex = 0;
        }

        protected bool IsComplete => CurrentIndex >= FullText.Length;

        protected char CurrentChar => FullText[CurrentIndex];

        protected string ReadQuotedString()
        {
            bool nextCharEscaped = false;
            char quoteDelimiterChar = CurrentChar;

            if (quoteDelimiterChar != '\'' && quoteDelimiterChar != '\"')
            {
                throw new MvxException("Error parsing string indexer - unexpected quote character {0} in text {1}",
                                       quoteDelimiterChar, FullText);
            }

            MoveNext();
            if (IsComplete)
            {
                throw new MvxException("Error parsing string indexer - unterminated in text {0}", FullText);
            }

            var textBuilder = new StringBuilder();
            while (true)
            {
                if (IsComplete)
                {
                    throw new MvxException("Error parsing string indexer - unterminated in text {0}", FullText);
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

            var text = textBuilder.ToString();
            return text;
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
                throw new MvxException("Unable to parse integer text from {0} in {1}", integerText, FullText);
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
                        throw new MvxException("\\U unicode character does not start with 0000 in {1}", FullText);
                    return ReadFourDigitUnicodeCharacter();
                default:
                    throw new MvxException("Sorry we don't currently support escaped characters like \\{0}",
                                           currentChar);
            }
        }

        private char ReadFourDigitUnicodeCharacter()
        {
            var digits = ReadNDigits(4);
            var number = UInt32.Parse(digits, NumberStyles.HexNumber);
            if (number > UInt16.MaxValue)
                throw new MvxException("\\u unicode character {0} out of range in {1}", number, FullText);
            return (char) number;
        }

        private string ReadNDigits(int count)
        {
            var toReturn = new StringBuilder(count);
            for (int i = 0; i < count; i++)
            {
                if (IsComplete)
                    throw new MvxException("Error while reading {0} of {1} digits in {2}", i + 1, count, FullText);

                var currentChar = CurrentChar;
                if (!char.IsDigit(currentChar))
                    throw new MvxException("Error while reading {0} of {1} digits in {2} - not a char {3}", i + 1, count,
                                           FullText, currentChar);

                toReturn.Append(currentChar);
                MoveNext();
            }

            return toReturn.ToString();
        }

        protected void MoveNext(uint increment = 1)
        {
            CurrentIndex += (int) increment;
        }

        protected void SkipWhitespaceAndCharacters(params char[] toSkip)
        {
            SkipWhitespaceAndCharacters((IEnumerable<char>) toSkip);
        }

        protected void SkipWhitespaceAndCharacters(IEnumerable<char> toSkip)
        {
            while (!IsComplete
                   && IsWhiteSpaceOrCharacter(CurrentChar, toSkip))
            {
                MoveNext();
            }
        }

        protected void SkipWhitespaceAndCharacters(Dictionary<char, bool> toSkip)
        {
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

        protected object ReadValue()
        {
            object toReturn;
            if (!TryReadValue(AllowNonQuotedText.Allow, out toReturn))
                throw new MvxException("Unable to read value");
            return toReturn;
        }

        public enum AllowNonQuotedText
        {
            Allow,
            DoNotAllow
        }

        protected bool TryReadValue(AllowNonQuotedText allowNonQuotedText, out object value)
        {
            SkipWhitespace();

            if (IsComplete)
            {
                throw new MvxException("Unexpected termination while reading value in {0}", FullText);
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
            if (peekString.Length < uppercaseKeyword.Length)
                return false;

            if (peekString.Length != uppercaseKeyword.Length
                && IsValidMidCharacterOfCSharpName(peekString[uppercaseKeyword.Length]))
                return false;

            if (!peekString.StartsWith(uppercaseKeyword))
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
            var safeLength = Math.Min(length, FullText.Length - CurrentIndex);
            if (safeLength == 0)
                return string.Empty;
            return FullText.Substring(CurrentIndex, safeLength);
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
                        throw new MvxException("Multiple decimal places seen in number in {0} at position {1}", FullText,
                                               CurrentIndex);
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
            return NumberFromText(numberText, numberText.Contains("."));
        }

        protected ValueType NumberFromText(string numberText, bool decimalPeriodSeen)
        {
            if (decimalPeriodSeen)
            {
                double doubleResult;
                if (double.TryParse(numberText,
                                    NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    out doubleResult))
                    return doubleResult;

                throw new MvxException("Failed to parse double from {0} in {1}", numberText, FullText);
            }
            else
            {
                // note that we use Int64 because Json.Net doe...
                Int64 intResult;
                if (Int64.TryParse(numberText,
                                   NumberStyles.AllowLeadingSign,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   out intResult))
                    return intResult;

                throw new MvxException("Failed to parse Int64 from {0} in {1}", numberText, FullText);
            }
        }

        protected object ReadEnumerationValue(Type enumerationType, bool ignoreCase = true)
        {
            var name = ReadValidCSharpName();
            try
            {
                return Enum.Parse(enumerationType, name, ignoreCase);
            }
            catch (ArgumentException exception)
            {
                throw exception.MvxWrap("Problem parsing {0} from {1} in {2}", enumerationType.Name, name, FullText);
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
                throw new MvxException("PropertyName must start with letter - position {0} in {1} - char {2}",
                                       CurrentIndex, FullText, firstChar);
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
            return char.IsLetter(firstChar) || (firstChar == '_');
        }

        protected bool IsValidMidCharacterOfCSharpName(char firstChar)
        {
            return char.IsLetterOrDigit(firstChar) || (firstChar == '_');
        }
    }
}