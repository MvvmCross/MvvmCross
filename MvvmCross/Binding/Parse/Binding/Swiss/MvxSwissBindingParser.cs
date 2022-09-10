// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Parse.Binding.Swiss
{
    public class MvxSwissBindingParser
        : MvxBindingParser
    {
        protected virtual IEnumerable<char> TerminatingCharacters()
        {
            return new[] { '=', ',', ';', '(', ')' };
        }

        private void ParsePath(string block, MvxSerializableBindingDescription description)
        {
            ParseEquals(block);
            ThrowExceptionIfPathAlreadyDefined(description);
            description.Path = ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
        }

        private void ParseConverter(string block, MvxSerializableBindingDescription description)
        {
            ParseEquals(block);
            var converter = ReadTargetPropertyName();
            if (!string.IsNullOrEmpty(description.Converter))
                MvxBindingLog.Warning("Overwriting existing Converter with {0}", converter);
            description.Converter = converter;
        }

        private void ParseConverterParameter(string block, MvxSerializableBindingDescription description)
        {
            ParseEquals(block);
            if (description.ConverterParameter != null)
                MvxBindingLog.Warning("Overwriting existing ConverterParameter");
            description.ConverterParameter = ReadValue();
        }

        private void ParseCommandParameter(string block, MvxSerializableBindingDescription description)
        {
            if (!IsComplete &&
               CurrentChar == '(')
            {
                // following https://github.com/MvvmCross/MvvmCross/issues/704, if the next character is "(" then
                // we can treat CommandParameter as a normal non-keyword block
                ParseNonKeywordBlockInto(description, block);
            }
            else
            {
                ParseEquals(block);
                if (!string.IsNullOrEmpty(description.Converter))
                    MvxBindingLog.Warning("Overwriting existing Converter with CommandParameter");
                description.Converter = "CommandParameter";
                description.ConverterParameter = ReadValue();
            }
        }

        private void ParseFallbackValue(string block, MvxSerializableBindingDescription description)
        {
            ParseEquals(block);
            if (description.FallbackValue != null)
                MvxBindingLog.Warning("Overwriting existing FallbackValue");
            description.FallbackValue = ReadValue();
        }

        private void ParseMode(string block, MvxSerializableBindingDescription description)
        {
            ParseEquals(block);
            description.Mode = ReadBindingMode();
        }

        protected virtual void ParseNextBindingDescriptionOptionInto(MvxSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            var block = ReadTextUntilNonQuotedOccurrenceOfAnyOf(TerminatingCharacters().ToArray());
            block = block.Trim();
            if (string.IsNullOrEmpty(block))
            {
                HandleEmptyBlock(description);
                return;
            }

            switch (block)
            {
                case "Path":
                    ParsePath(block, description);
                    break;
                case "Converter":
                    ParseConverter(block, description);
                    break;
                case "ConverterParameter":
                    ParseConverterParameter(block, description);
                    break;
                case "CommandParameter":
                    ParseCommandParameter(block, description);
                    break;
                case "FallbackValue":
                    ParseFallbackValue(block, description);
                    break;
                case "Mode":
                    ParseMode(block, description);
                    break;
                default:
                    ParseNonKeywordBlockInto(description, block);
                    break;
            }
        }

        protected virtual void HandleEmptyBlock(MvxSerializableBindingDescription description)
        {
            // default implementation doesn't do any special handling on an empty block
        }

        protected virtual void ParseNonKeywordBlockInto(MvxSerializableBindingDescription description, string block)
        {
            if (!IsComplete && CurrentChar == '(')
            {
                ParseFunctionStyleBlockInto(description, block);
            }
            else
            {
                ThrowExceptionIfPathAlreadyDefined(description);
                description.Path = block;
            }
        }

        protected virtual void ParseFunctionStyleBlockInto(MvxSerializableBindingDescription description, string block)
        {
            description.Converter = block;
            MoveNext();
            if (IsComplete)
                throw new MvxException("Unterminated () pair for converter {0}", block);

            ParseChildBindingDescriptionInto(description);
            SkipWhitespace();
            switch (CurrentChar)
            {
                case ')':
                    MoveNext();
                    break;

                case ',':
                    MoveNext();
                    ReadConverterParameterAndClosingBracket(description);
                    break;

                default:
                    throw new MvxException("Unexpected character {0} while parsing () contents", CurrentChar);
            }
        }

        protected void ReadConverterParameterAndClosingBracket(MvxSerializableBindingDescription description)
        {
            SkipWhitespace();
            description.ConverterParameter = ReadValue();
            SkipWhitespace();
            if (CurrentChar != ')')
                throw new MvxException("Unterminated () pair for converter {0}");
            MoveNext();
        }

        protected void ParseChildBindingDescriptionInto(MvxSerializableBindingDescription description,
            ParentIsLookingForComma parentIsLookingForComma = ParentIsLookingForComma.ParentIsLookingForComma)
        {
            SkipWhitespace();
            description.Function = "Single";
            description.Sources = new[]
                {
                    ParseBindingDescription(parentIsLookingForComma)
                };
        }

        protected void ThrowExceptionIfPathAlreadyDefined(MvxSerializableBindingDescription description)
        {
            if (description.Path != null &&
                description.Literal != null &&
                description.Function != null)
            {
                throw new MvxException(
                    "Make sure you are using ';' to separate multiple bindings. You cannot specify Path/Literal/Combiner more than once - position {0} in {1}",
                    CurrentIndex, FullText);
            }
        }

        protected enum ParentIsLookingForComma
        {
            ParentIsLookingForComma,
            ParentIsNotLookingForComma
        }

        protected override MvxSerializableBindingDescription ParseBindingDescription() =>
            ParseBindingDescription(ParentIsLookingForComma.ParentIsNotLookingForComma);

        protected virtual MvxSerializableBindingDescription ParseBindingDescription(
            ParentIsLookingForComma parentIsLookingForComma)
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
                        if (parentIsLookingForComma == ParentIsLookingForComma.ParentIsLookingForComma)
                            return description;

                        MoveNext();
                        break;

                    case ';':
                    case ')':
                        return description;

                    default:
                        if (DetectOperator())
                            ParseOperatorWithLeftHand(description);
                        else
                            throw new MvxException(
                                "Unexpected character {0} at position {1} in {2} - expected string-end, ',' or ';'",
                                CurrentChar,
                                CurrentIndex,
                                FullText);
                        break;
                }
            }
        }

        protected virtual MvxSerializableBindingDescription ParseOperatorWithLeftHand(
            MvxSerializableBindingDescription description)
        {
            throw new MvxException("Operators not expected in base SwissBinding");
        }

        protected virtual bool DetectOperator() => false;
    }
}
