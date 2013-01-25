// MvxOldSwissBindingParser.cs
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
    [Obsolete]
    public class MvxOldSwissBindingParser 
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

        private MvxSerializableBindingDescription ParseBindingDescription()
        {
            var description = new MvxSerializableBindingDescription();
            description.Mode = ReadOptionalBindingMode();
            SkipWhitespace();
            description.Path = ReadSourcePath();
            SkipWhitespace();

            if (IsComplete)
                return description;

            var currentChar = CurrentChar;
            char closingBrace;
            switch (currentChar)
            {
                case '(':
                    closingBrace = ')';
                    break;
                case '[':
                    closingBrace = ']';
                    break;
                case '{':
                    closingBrace = ')';
                    break;
                default:
                    return description;
            }

            MoveNext();

            while (true)
            {
                SkipWhitespace();

                if (IsComplete)
                {
                    throw new MvxException("Unterminated options seen in binding {0}", FullText);
                }

                if (CurrentChar == closingBrace)
                {
                    MoveNext();
                    return description;
                }

                ParseOptionalBindingField(description);

                SkipWhitespaceAndDescriptionSeparators();
            }
        }

        protected bool CurrentCharIsClosingBrace()
        {
            return CurrentChar == ')';
        }

        protected void ParseOptionalBindingField(MvxSerializableBindingDescription description)
        {
            var optionName = ReadValidCSharpName();
            SkipWhitespace();
            if(IsComplete)
                throw new MvxException("Invalid termination of binding while reading optional binding property in {0}", FullText);
            
            if (CurrentChar != '=')
                throw new MvxException("Missing = in binding - character seen is {0} in {1}", CurrentChar, FullText);

            MoveNext();

            switch (optionName)
            {
                case "Converter":
                    var converterName = ReadValidCSharpName();
                    description.Converter = converterName;
                    break;

                case "ConverterParameter":
                    var converterParameter = ReadValue();
                    description.ConverterParameter = converterParameter;
                    break;

                case "FallbackValue":
                case "DefaultValue":
                    var fallbackValue = ReadValue();
                    description.FallbackValue = fallbackValue;
                    break;

                case "Mode":
                    if (description.Mode != MvxBindingMode.Default)
                    {
                        MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Mode specified multiple times in binding in {0} - for readability either use <,>,<1,<> or use (Mode=...) - not both", FullText);
                    }
                    description.Mode = ReadBindingMode();
                    break;

                default:
                    throw new MvxException("Unknown binding option {0} in {1}", optionName, FullText);
            }
        }

        protected MvxBindingMode ReadBindingMode()
        {
            return (MvxBindingMode)ReadEnumerationValue(typeof (MvxBindingMode));
        }

        protected string ReadSourcePath()
        {
            SkipWhitespace();
            return ReadTextUntilNonQuotedOccurrenceOfAnyOf('(', ';', ',');
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

        protected MvxBindingMode ReadOptionalBindingMode()
        {
            SkipWhitespace();
            var firstChar = CurrentChar;
            switch (firstChar)
            {
                case '<':
                    MoveNext();
                    var secondChar = CurrentChar;
                    switch (secondChar)
                    {
                        case '1':
                            MoveNext();
                            return MvxBindingMode.OneTime;
                        case '>':
                            MoveNext();
                            return MvxBindingMode.TwoWay;
                        default:
                            // char was not for binding - so do not perform a second MoveNext()
                            return MvxBindingMode.OneWay;
                    }

                case '>':
                    MoveNext();
                    return MvxBindingMode.OneWayToSource;

                case '=':
                    MoveNext();
                    return MvxBindingMode.Default;

                default:
                    // char was not for binding - so do not MoveNext()
                    return MvxBindingMode.Default;
            }
        }

        protected string ReadTargetPropertyName()
        {
            return ReadValidCSharpName();
        }

        protected void SkipWhitespaceAndDescriptionSeparators()
        {
            SkipWhitespaceAndCharacters(',', ';');
        }
    }
}