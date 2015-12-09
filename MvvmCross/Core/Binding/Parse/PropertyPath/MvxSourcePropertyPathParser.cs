// MvxSourcePropertyPathParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Parse.PropertyPath
{
    using System.Collections.Generic;
    using System.Text;

    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Parse;

    public class MvxSourcePropertyPathParser
        : MvxParser
          , IMvxSourcePropertyPathParser
    {
        protected List<MvxPropertyToken> CurrentTokens { get; private set; }

        protected override void Reset(string textToParse)
        {
            textToParse = this.MakeSafe(textToParse);
            this.CurrentTokens = new List<MvxPropertyToken>();
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

        public IList<MvxPropertyToken> Parse(string textToParse)
        {
            this.Reset(textToParse);

            while (!this.IsComplete)
            {
                this.ParseNextToken();
            }

            if (this.CurrentTokens.Count == 0)
            {
                this.CurrentTokens.Add(new MvxEmptyPropertyToken());
            }

            return this.CurrentTokens;
        }

        private void ParseNextToken()
        {
            this.SkipWhitespaceAndPeriods();

            if (this.IsComplete)
            {
                return;
            }

            var currentChar = this.CurrentChar;
            if (currentChar == '[')
            {
                this.ParseIndexer();
            }
            else if (char.IsLetter(currentChar) || currentChar == '_')
            {
                this.ParsePropertyName();
            }
            else
            {
                throw new MvxException("Unexpected character {0} at position {1} in targetProperty text {2}",
                                       currentChar,
                                       this.CurrentIndex, this.FullText);
            }
        }

        private void ParsePropertyName()
        {
            var propertyText = new StringBuilder();
            while (!this.IsComplete)
            {
                var currentChar = this.CurrentChar;
                if (!char.IsLetterOrDigit(currentChar) && currentChar != '_')
                    break;
                propertyText.Append(currentChar);
                this.MoveNext();
            }

            var text = propertyText.ToString();
            this.CurrentTokens.Add(new MvxPropertyNamePropertyToken(text));
        }

        private void ParseIndexer()
        {
            if (this.CurrentChar != '[')
            {
                throw new MvxException(
                    "Internal error - ParseIndexer should only be called with a string starting with [");
            }

            this.MoveNext();
            if (this.IsComplete)
            {
                throw new MvxException("Invalid indexer targetProperty text {0}", this.FullText);
            }

            this.SkipWhitespaceAndPeriods();

            if (this.IsComplete)
            {
                throw new MvxException("Invalid indexer targetProperty text {0}", this.FullText);
            }

            if (this.CurrentChar == '\'' || this.CurrentChar == '\"')
            {
                this.ParseQuotedStringIndexer();
            }
            else if (char.IsDigit(this.CurrentChar))
            {
                this.ParseIntegerIndexer();
            }
            else
            {
                this.ParseUnquotedStringIndexer();
            }

            this.SkipWhitespaceAndPeriods();
            if (this.IsComplete)
            {
                throw new MvxException("Invalid termination of indexer targetProperty text in {0}", this.FullText);
            }

            if (this.CurrentChar != ']')
            {
                throw new MvxException(
                    "Unexpected character {0} at position {1} in targetProperty text {2} - expected terminator",
                    this.CurrentChar,
                    this.CurrentIndex, this.FullText);
            }

            this.MoveNext();
        }

        private void ParseIntegerIndexer()
        {
            var index = (int)this.ReadUnsignedInteger();
            this.CurrentTokens.Add(new MvxIntegerIndexerPropertyToken(index));
        }

        private void ParseQuotedStringIndexer()
        {
            var text = this.ReadQuotedString();
            this.CurrentTokens.Add(new MvxStringIndexerPropertyToken(text));
        }

        private void ParseUnquotedStringIndexer()
        {
            var text = this.ReadTextUntil(']');
            this.CurrentTokens.Add(new MvxStringIndexerPropertyToken(text));
        }

        private void SkipWhitespaceAndPeriods()
        {
            this.SkipWhitespaceAndCharacters(new[] { '.' });
        }
    }
}