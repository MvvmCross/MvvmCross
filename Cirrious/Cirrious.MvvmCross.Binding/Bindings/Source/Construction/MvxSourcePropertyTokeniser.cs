// MvxSourcePropertyTokeniser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Text;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction.PropertyTokens;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Parser;
using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Construction
{
    public class MvxSourcePropertyTokeniser 
        : MvxBaseTokeniser<MvxBasePropertyToken>
        , IMvxSourcePropertyTokeniser
    {
        public IList<MvxBasePropertyToken> Tokenise(string textToTokenise)
        {
            Reset(textToTokenise);

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
            CurrentTokens.Add(new MvxPropertyNamePropertyToken(propertyText.ToString()));
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
                    CurrentChar, CurrentIndex, FullText);
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
            var index = ReadUnsignedInteger();
            CurrentTokens.Add(new MvxIntegerIndexerPropertyToken(index));
        }

        private void ParseStringIndexer()
        {
            var text = ReadQuotedString();
            CurrentTokens.Add(new MvxStringIndexerPropertyToken(text));
        }

        private void SkipWhitespaceAndPeriods()
        {
            SkipWhitespaceAndCharacters(new [] { '.' });
        }
    }
}