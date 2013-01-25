// MvxSwissBindingParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Parse.Binding.Swiss
{
    public class MvxSwissBindingParser
        : MvxBaseParser
        , IMvxBindingParser
    {
        public bool TryParseBindingDescription(string text, out MvxSerializableBindingDescription requestedDescription)
        {
            try
            {
                Reset(text);
                requestedDescription = this.ParseBindingDescription();
                return true;
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                        "Problem parsing Swiss binding {0}", exception.ToLongString());
                requestedDescription = null;
                return false;
            }
        }

        public bool TryParseBindingSpecification(string text, out MvxSerializableBindingSpecification requestedBindings)
        {
            try
            {
                Reset(text);

                var toReturn = new MvxSerializableBindingSpecification();
                while (!IsComplete)
                {
                    SkipWhitespaceAndDescriptionSeparators();
                    var result = ParseTargetPropertyNameAndDescription();
                    toReturn[result.Key] = result.Value;
                    SkipWhitespaceAndDescriptionSeparators();
                }

                requestedBindings = toReturn;
                return true;
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                        "Problem parsing Swiss binding {0}", exception.ToLongString());
                requestedBindings = null;
                return false;
            }
        }

        private KeyValuePair<string, MvxSerializableBindingDescription> ParseTargetPropertyNameAndDescription()
        {
            var targetPropertyName = ReadTargetPropertyName();
            SkipWhitespace();
            var description = ParseBindingDescription();
            return new KeyValuePair<string, MvxSerializableBindingDescription>(targetPropertyName, description);
        }

        private void ParseNextBindingDescriptionOptionInto(MvxSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            var block = ReadTextUntilNonQuotedOccurrenceOfAnyOf('=', ',', ';');
            block = block.Trim();
            if (string.IsNullOrEmpty(block))
            {
                return;
            }

            switch (block)
            {
                case "Path":
                    ParseEquals(block);
                    description.Path  = ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
                    break;
                case "Converter":
                    ParseEquals(block);
                    description.Converter = ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
                    break;
                case "ConverterParameter":
                    ParseEquals(block);
                    description.ConverterParameter = ReadValue();
                    break;
                case "FallbackValue":
                    ParseEquals(block);
                    description.FallbackValue = ReadValue();
                    break;
                case "Mode":
                    ParseEquals(block);
                    //if (description.Mode != MvxBindingMode.Default)
                    //{
                    //    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Mode specified multiple times in binding in {0} - for readability either use <,>,<1,<> or use (Mode=...) - not both", FullText);
                    //}
                    description.Mode = ReadBindingMode();
                    break;
                default:
                    if (!string.IsNullOrEmpty(description.Path))
                    {
                        throw new MvxException("You cannot specify Path more than once - first Path '{0}', second Path '{1}', position {2} in {3}", description.Path, block, CurrentIndex, FullText);
                    }
                    description.Path = block;
                    break;
            }
        }

        private void ParseEquals(string block)
        {
            if (IsComplete)
                throw new MvxException("Cannot terminate binding expression during option {0} in {1}",
                                       block,
                                       FullText);
            if (CurrentChar != '=')
                throw new MvxException("Must follow binding option {0} with an '=' in {1}",
                                       block,
                                       FullText);

            MoveNext();
            if (IsComplete)
                throw new MvxException("Cannot terminate binding expression during option {0} in {1}",
                                       block,
                                       FullText);
        }

        private MvxSerializableBindingDescription ParseBindingDescription()
        {
            var description = new MvxSerializableBindingDescription();
            SkipWhitespace();

            while (true)
            {
                ParseNextBindingDescriptionOptionInto(description);

                SkipWhitespace();
                if (IsComplete)
                    return description;

                switch (CurrentChar)
                {
                    case ',':
                        MoveNext();
                        break;
                    case ';':
                        return description;
                    default:
                        throw new MvxException("Unexpected character {0} at position {1} in {2} - expected string-end, ',' or ';'",
                            CurrentChar,
                            CurrentIndex,
                            FullText);
                }
            }
        }

        protected MvxBindingMode ReadBindingMode()
        {
            return (MvxBindingMode)ReadEnumerationValue(typeof(MvxBindingMode));
        }

        protected string ReadTextUntilNonQuotedOccurrenceOfAnyOf(params char[] terminationCharacters)
        {
            var terminationLookup = terminationCharacters.ToDictionary(c => c, c => true);
            SkipWhitespace();
            var toReturn = new StringBuilder();

            while (!IsComplete)
            {
                var currentChar = CurrentChar;
                if (currentChar == '\'' || currentChar == '\"')
                {
                    var subText = ReadQuotedString();
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
                MoveNext();
            }

            return toReturn.ToString();
        }

        protected string ReadTargetPropertyName()
        {
            return ReadValidCSharpName();
        }

        protected void SkipWhitespaceAndOptionSeparators()
        {
            SkipWhitespaceAndCharacters(',');
        }

        protected void SkipWhitespaceAndDescriptionSeparators()
        {
            SkipWhitespaceAndCharacters(';');
        }
    }
}