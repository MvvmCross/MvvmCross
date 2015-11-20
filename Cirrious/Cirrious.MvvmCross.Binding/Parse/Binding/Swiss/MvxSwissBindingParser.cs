// MvxSwissBindingParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Parse.Binding.Swiss
{
    public class MvxSwissBindingParser
        : MvxBindingParser
    {
        protected virtual IEnumerable<char> TerminatingCharacters()
        {
            return new[] { '=', ',', ';', '(', ')' };
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
                    ParseEquals(block);
                    ThrowExceptionIfPathAlreadyDefined(description);
                    description.Path = ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
                    break;

                case "Converter":
                    ParseEquals(block);
                    var converter = ReadTargetPropertyName();
                    if (!string.IsNullOrEmpty(description.Converter))
                        MvxBindingTrace.Warning("Overwriting existing Converter with {0}", converter);
                    description.Converter = converter;
                    break;

                case "ConverterParameter":
                    ParseEquals(block);
                    if (description.ConverterParameter != null)
                        MvxBindingTrace.Warning("Overwriting existing ConverterParameter");
                    description.ConverterParameter = ReadValue();
                    break;

                case "CommandParameter":
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
                            MvxBindingTrace.Warning("Overwriting existing Converter with CommandParameter");
                        description.Converter = "CommandParameter";
                        description.ConverterParameter = ReadValue();
                    }
                    break;

                case "FallbackValue":
                    ParseEquals(block);
                    if (description.FallbackValue != null)
                        MvxBindingTrace.Warning("Overwriting existing FallbackValue");
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
                                                        ParentIsLookingForComma parentIsLookingForComma =
                                                            ParentIsLookingForComma.ParentIsLookingForComma)
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
            if (description.Path != null
                && description.Literal != null
                && description.Function != null)
            {
                throw new MvxException(
                    "Make sure you are using ';' to separate multiple bindings. You cannot specify Path/Literal/Combiner more than once - position {0} in {1}",
                    CurrentIndex, FullText);
            }
        }

        protected override MvxSerializableBindingDescription ParseBindingDescription()
        {
            return ParseBindingDescription(ParentIsLookingForComma.ParentIsNotLookingForComma);
        }

        protected enum ParentIsLookingForComma
        {
            ParentIsLookingForComma,
            ParentIsNotLookingForComma
        }

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

        protected virtual bool DetectOperator()
        {
            return false;
        }
    }
}