// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text;
using MvvmCross.Base;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Parse.PropertyPath
{
    public class MvxPropertyPathParser : MvxParser
    {
        protected List<MvxPropertyToken> CurrentTokens { get; } = new List<MvxPropertyToken>();

        protected override void Reset(string textToParse)
        {
            CurrentTokens.Clear();
            textToParse = MakeSafe(textToParse);
            base.Reset(textToParse);
        }

        public static string MakeSafe(string textToParse)
        {
            if (textToParse == null)
                return string.Empty;
            if (textToParse.Trim() == ".")
                return string.Empty;
            return textToParse;
        }

        public IList<MvxPropertyToken> Parse(string textToParse)
        {
            Reset(textToParse);

            while (!IsComplete)
            {
                ParseNextToken();
            }

            if (CurrentTokens.Count == 0)
            {
                CurrentTokens.Add(new MvxEmptyPropertyToken());
            }

            return CurrentTokens;
        }

        private void ParseNextToken()
        {
            SkipWhitespaceAndPeriods();

            if (IsComplete)
            {
                return;
            }

            var currentChar = CurrentChar;
            if (currentChar == '[')
            {
                ParseIndexer();
            }
            else if (char.IsLetter(currentChar) || currentChar == '_')
            {
                ParsePropertyName();
            }
            else
            {
                throw new MvxException("Unexpected character {0} at position {1} in targetProperty text {2}",
                    currentChar,
                    CurrentIndex, FullText);
            }
        }

        private void ParsePropertyName()
        {
            var propertyText = new StringBuilder();
            while (!IsComplete)
            {
                var currentChar = CurrentChar;
                if (!char.IsLetterOrDigit(currentChar) && currentChar != '_')
                    break;
                propertyText.Append(currentChar);
                MoveNext();
            }

            var text = propertyText.ToString();
            CurrentTokens.Add(new MvxPropertyNamePropertyToken(text));
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
                throw new MvxException("Invalid indexer targetProperty text {0}", FullText);
            }

            SkipWhitespaceAndPeriods();

            if (IsComplete)
            {
                throw new MvxException("Invalid indexer targetProperty text {0}", FullText);
            }

            if (CurrentChar == '\'' || CurrentChar == '\"')
            {
                ParseQuotedStringIndexer();
            }
            else if (char.IsDigit(CurrentChar))
            {
                ParseIntegerIndexer();
            }
            else
            {
                ParseUnquotedStringIndexer();
            }

            SkipWhitespaceAndPeriods();
            if (IsComplete)
            {
                throw new MvxException("Invalid termination of indexer targetProperty text in {0}", FullText);
            }

            if (CurrentChar != ']')
            {
                throw new MvxException(
                    "Unexpected character {0} at position {1} in targetProperty text {2} - expected terminator",
                    CurrentChar,
                    CurrentIndex, FullText);
            }

            MoveNext();
        }

        private void ParseIntegerIndexer()
        {
            var index = (int)ReadUnsignedInteger();
            CurrentTokens.Add(new MvxIntegerIndexerPropertyToken(index));
        }

        private void ParseQuotedStringIndexer()
        {
            var text = ReadQuotedString();
            CurrentTokens.Add(new MvxStringIndexerPropertyToken(text));
        }

        private void ParseUnquotedStringIndexer()
        {
            var text = ReadTextUntil(']');
            CurrentTokens.Add(new MvxStringIndexerPropertyToken(text));
        }

        private void SkipWhitespaceAndPeriods()
        {
            SkipWhitespaceAndCharacters('.');
        }
    }
}
