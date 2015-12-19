// MvxBindingParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Parse.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Parse;
    using MvvmCross.Platform.Platform;

    public abstract class MvxBindingParser
        : MvxParser
          , IMvxBindingParser
    {
        protected abstract MvxSerializableBindingDescription ParseBindingDescription();

        public bool TryParseBindingDescription(string text, out MvxSerializableBindingDescription requestedDescription)
        {
            try
            {
                this.Reset(text);
                requestedDescription = this.ParseBindingDescription();
                return true;
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Problem parsing binding {0}", exception.ToLongString());
                requestedDescription = null;
                return false;
            }
        }

        public bool TryParseBindingSpecification(string text, out MvxSerializableBindingSpecification requestedBindings)
        {
            try
            {
                this.Reset(text);

                var toReturn = new MvxSerializableBindingSpecification();
                while (!this.IsComplete)
                {
                    this.SkipWhitespaceAndDescriptionSeparators();
                    var result = this.ParseTargetPropertyNameAndDescription();
                    toReturn[result.Key] = result.Value;
                    this.SkipWhitespaceAndDescriptionSeparators();
                }

                requestedBindings = toReturn;
                return true;
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Problem parsing binding {0}", exception.ToLongString());
                requestedBindings = null;
                return false;
            }
        }

        protected KeyValuePair<string, MvxSerializableBindingDescription> ParseTargetPropertyNameAndDescription()
        {
            var targetPropertyName = this.ReadTargetPropertyName();
            this.SkipWhitespace();
            var description = this.ParseBindingDescription();
            return new KeyValuePair<string, MvxSerializableBindingDescription>(targetPropertyName, description);
        }

        protected void ParseEquals(string block)
        {
            if (this.IsComplete)
                throw new MvxException("Cannot terminate binding expression during option {0} in {1}",
                                       block,
                                       this.FullText);
            if (this.CurrentChar != '=')
                throw new MvxException("Must follow binding option {0} with an '=' in {1}",
                                       block,
                                       this.FullText);

            this.MoveNext();
            if (this.IsComplete)
                throw new MvxException("Cannot terminate binding expression during option {0} in {1}",
                                       block,
                                       this.FullText);
        }

        protected MvxBindingMode ReadBindingMode()
        {
            return (MvxBindingMode)this.ReadEnumerationValue(typeof(MvxBindingMode));
        }

        protected string ReadTextUntilNonQuotedOccurrenceOfAnyOf(params char[] terminationCharacters)
        {
            var terminationLookup = terminationCharacters.ToDictionary(c => c, c => true);
            this.SkipWhitespace();
            var toReturn = new StringBuilder();

            while (!this.IsComplete)
            {
                var currentChar = this.CurrentChar;
                if (currentChar == '\'' || currentChar == '\"')
                {
                    var subText = this.ReadQuotedString();
                    toReturn.Append(currentChar);
                    toReturn.Append(subText);
                    toReturn.Append(currentChar);
                    continue;
                }

                if (terminationLookup.ContainsKey(currentChar))
                {
                    break;
                }

                toReturn.Append(currentChar);
                this.MoveNext();
            }

            return toReturn.ToString();
        }

        protected string ReadTargetPropertyName()
        {
            return this.ReadValidCSharpName();
        }

        protected void SkipWhitespaceAndOptionSeparators()
        {
            this.SkipWhitespaceAndCharacters(',');
        }

        protected void SkipWhitespaceAndDescriptionSeparators()
        {
            this.SkipWhitespaceAndCharacters(';');
        }
    }
}