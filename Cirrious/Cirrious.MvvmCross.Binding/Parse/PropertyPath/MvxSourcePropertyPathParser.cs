// MvxSourcePropertyPathParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Text;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Parse.PropertyPath
{
    public class MvxSourcePropertyPathParser
        : MvxBaseParser
          , IMvxSourcePropertyPathParser
    {
        protected List<MvxBasePropertyToken> CurrentTokens { get; private set; }

        protected override void Reset(string textToParse)
        {
            textToParse = MakeSafe(textToParse);
            CurrentTokens = new List<MvxBasePropertyToken>();
            base.Reset(textToParse);
        }

        private string MakeSafe(string textToParse)
        {
            if (textToParse == null)
                return string.Empty;
            if (textToParse.Trim() == ".")
                return string.Empty;
            return textToParse;
        }

        public IList<MvxBasePropertyToken> Parse(string textToParse)
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
                                       CurrentIndex, FullText);
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
                throw new MvxException("Invalid indexer property text {0}", FullText);
            }

            SkipWhitespaceAndPeriods();

            if (IsComplete)
            {
                throw new MvxException("Invalid indexer property text {0}", FullText);
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
                throw new MvxException("Invalid termination of indexer property text in {0}", FullText);
            }

            if (CurrentChar != ']')
            {
                throw new MvxException(
                    "Unexpected character {0} at position {1} in property text {2} - expected terminator", CurrentChar,
                    CurrentIndex, FullText);
            }

            MoveNext();
        }

        private void ParseIntegerIndexer()
        {
#warning Need to tidy this up so that it properly reads signed integers too
            var index = (int) ReadUnsignedInteger();
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
            SkipWhitespaceAndCharacters(new[] {'.'});
        }
    }
}