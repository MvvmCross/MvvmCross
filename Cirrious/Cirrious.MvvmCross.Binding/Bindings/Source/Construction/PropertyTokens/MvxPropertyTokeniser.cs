// MvxPropertyTokeniser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Text;
using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Construction.PropertyTokens
{
    public class MvxPropertyTokeniser : IMvxPropertyTokeniser
    {
        private string _fullText;
        private int _currentIndex;
        private List<MvxBasePropertyToken> _currentResult;

        public IList<MvxBasePropertyToken> Tokenise(string toParse)
        {
            _fullText = toParse;
            _currentIndex = 0;
            _currentResult = new List<MvxBasePropertyToken>();

            while (!IsComplete)
            {
                ParseNextToken();
            }

            if (_currentResult.Count == 0)
            {
                _currentResult.Add(new MvxEmptyPropertyToken());
            }

            return _currentResult;
        }

        private void ParseNextToken()
        {
            SkipWhitespaceAndPeriods();

            if (IsComplete)
            {
                return;
            }

            if (CurrentChar == '[')
            {
                ParseIndexer();
            }
            else if (char.IsLetter(CurrentChar))
            {
                ParsePropertyName();
            }
            else
            {
                throw new MvxException("Unexpected character {0} at position {1} in property text {2}", CurrentChar,
                                       _currentIndex, _fullText);
            }
        }

        private void ParsePropertyName()
        {
            var propertyText = new StringBuilder();
            while (!IsComplete && char.IsLetterOrDigit(CurrentChar))
            {
                propertyText.Append(CurrentChar);
                MoveNext();
            }
            _currentResult.Add(new MvxPropertyNamePropertyToken(propertyText.ToString()));
        }

        private void ParseIndexer()
        {
            if (CurrentChar != '[')
            {
                throw new MvxException(
                    "Internal error - ParseIndexer should only be called with a string starting with [");
            }

            MoveNext();
            if (IsComplete)
            {
                throw new MvxException("Invalid indexer property text {0}", _fullText);
            }

            SkipWhitespaceAndPeriods();

            if (IsComplete)
            {
                throw new MvxException("Invalid indexer property text {0}", _fullText);
            }

            if (CurrentChar == '\'' || CurrentChar == '\"')
            {
                ParseStringIndexer();
            }
            else if (char.IsDigit(CurrentChar))
            {
                ParseIntegerIndexer();
            }
            else
            {
                throw new MvxException(
                    "Unexpected character {0} at position {1} in property text {2} - expected terminator start of string or number",
                    CurrentChar, _currentIndex, _fullText);
            }

            SkipWhitespaceAndPeriods();
            if (IsComplete)
            {
                throw new MvxException("Invalid termination of indexer property text in {0}", _fullText);
            }

            if (CurrentChar != ']')
            {
                throw new MvxException(
                    "Unexpected character {0} at position {1} in property text {2} - expected terminator", CurrentChar,
                    _currentIndex, _fullText);
            }

            MoveNext();
        }

        private void ParseIntegerIndexer()
        {
            var integerStringBuilder = new StringBuilder();
            while (!IsComplete && char.IsDigit(CurrentChar))
            {
                integerStringBuilder.Append(CurrentChar);
                MoveNext();
            }
            int index;
            var integerText = integerStringBuilder.ToString();
            if (!int.TryParse(integerText, out index))
            {
                throw new MvxException("Unable to parse integer text from {0} in {1}", integerText, _fullText);
            }

            _currentResult.Add(new MvxIntegerIndexerPropertyToken(index));
        }

        private void ParseStringIndexer()
        {
            bool nextCharacterEscaped = false;
            char quoteCharacter = CurrentChar;

            if (quoteCharacter != '\'' && quoteCharacter != '\"')
            {
                throw new MvxException("Error parsing string indexer - unexpected quote character {0} in text {1}",
                                       quoteCharacter, _fullText);
            }

            MoveNext();
            if (IsComplete)
            {
                throw new MvxException("Error parsing string indexer - unterminated in text {0}", _fullText);
            }

            var textBuilder = new StringBuilder();
            while (true)
            {
                if (IsComplete)
                {
                    throw new MvxException("Error parsing string indexer - unterminated in text {0}", _fullText);
                }

                var currentChar = CurrentChar;

                if (nextCharacterEscaped)
                {
                    char charToInsert;
                    switch (currentChar)
                    {
#warning Check this list! Possibly also need unicode codes and other stuff here?
                        case 't':
                            charToInsert = '\t';
                            break;
                        case 'r':
                            charToInsert = '\r';
                            break;
                        case 'n':
                            charToInsert = '\n';
                            break;
                        case '\'':
                            charToInsert = '\'';
                            break;
                        case '\"':
                            charToInsert = '\"';
                            break;
                        case '\\':
                            charToInsert = '\\';
                            break;
                        default:
                            throw new MvxException("Sorry we don't currently support escaped characters like \\{0}",
                                                   currentChar);
                    }
                    textBuilder.Append(charToInsert);
                    nextCharacterEscaped = false;
                    MoveNext();
                    continue;
                }

                if (currentChar == '\\')
                {
                    nextCharacterEscaped = true;
                    MoveNext();
                    continue;
                }

                if (currentChar == quoteCharacter)
                {
                    MoveNext();
                    break;
                }

                textBuilder.Append(currentChar);
                MoveNext();
            }

            var text = textBuilder.ToString();
            _currentResult.Add(new MvxStringIndexerPropertyToken(text));
        }

        private void SkipWhitespaceAndPeriods()
        {
            while (!IsComplete
                   && IsWhiteSpaceOrPeriod(CurrentChar))
            {
                MoveNext();
            }
        }

        private static bool IsWhiteSpaceOrPeriod(char toTest)
        {
            return char.IsWhiteSpace(toTest) || toTest == '.';
        }

        private bool MoveNext()
        {
            _currentIndex++;
            return IsComplete;
        }

        private bool IsComplete
        {
            get { return _currentIndex >= _fullText.Length; }
        }

        private char CurrentChar
        {
            get { return _fullText[_currentIndex]; }
        }
    }
}