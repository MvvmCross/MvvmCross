// MvxBaseTokeniser.cs
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
using Cirrious.MvvmCross.Exceptions;

#warning Where should this class go really? Wrong namespace...
namespace Cirrious.MvvmCross.Binding.Parser
{
    public abstract class MvxBaseTokeniser<TToken>
    {
        protected string FullText { get; private set; }
        protected int CurrentIndex { get; private set; }
        protected List<TToken> CurrentTokens { get; private set; }

        protected void Reset(string textToParse)
        {
            FullText = textToParse;
            CurrentIndex = 0;
            CurrentTokens = new List<TToken>();
        }

        protected bool IsComplete
        {
            get { return CurrentIndex >= FullText.Length; }
        }

        protected char CurrentChar
        {
            get { return FullText[CurrentIndex]; }
        }

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
            return (char)number;
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
                    throw new MvxException("Error while reading {0} of {1} digits in {2} - not a char {3}", i + 1, count, FullText, currentChar);

                toReturn.Append((char) currentChar);
                MoveNext();
            }

            return toReturn.ToString();
        }

        protected void MoveNext()
        {
            CurrentIndex++;
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
    }
}