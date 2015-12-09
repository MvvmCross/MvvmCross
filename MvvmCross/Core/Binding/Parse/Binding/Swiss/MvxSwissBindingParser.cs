// MvxSwissBindingParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Parse.Binding.Swiss
{
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Platform.Exceptions;

    public class MvxSwissBindingParser
        : MvxBindingParser
    {
        protected virtual IEnumerable<char> TerminatingCharacters()
        {
            return new[] { '=', ',', ';', '(', ')' };
        }

        protected virtual void ParseNextBindingDescriptionOptionInto(MvxSerializableBindingDescription description)
        {
            if (this.IsComplete)
                return;

            var block = this.ReadTextUntilNonQuotedOccurrenceOfAnyOf(this.TerminatingCharacters().ToArray());
            block = block.Trim();
            if (string.IsNullOrEmpty(block))
            {
                this.HandleEmptyBlock(description);
                return;
            }

            switch (block)
            {
                case "Path":
                    this.ParseEquals(block);
                    this.ThrowExceptionIfPathAlreadyDefined(description);
                    description.Path = this.ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
                    break;

                case "Converter":
                    this.ParseEquals(block);
                    var converter = this.ReadTargetPropertyName();
                    if (!string.IsNullOrEmpty(description.Converter))
                        MvxBindingTrace.Warning("Overwriting existing Converter with {0}", converter);
                    description.Converter = converter;
                    break;

                case "ConverterParameter":
                    this.ParseEquals(block);
                    if (description.ConverterParameter != null)
                        MvxBindingTrace.Warning("Overwriting existing ConverterParameter");
                    description.ConverterParameter = this.ReadValue();
                    break;

                case "CommandParameter":
                    if (!this.IsComplete &&
                        this.CurrentChar == '(')
                    {
                        // following https://github.com/MvvmCross/MvvmCross/issues/704, if the next character is "(" then
                        // we can treat CommandParameter as a normal non-keyword block
                        this.ParseNonKeywordBlockInto(description, block);
                    }
                    else
                    {
                        this.ParseEquals(block);
                        if (!string.IsNullOrEmpty(description.Converter))
                            MvxBindingTrace.Warning("Overwriting existing Converter with CommandParameter");
                        description.Converter = "CommandParameter";
                        description.ConverterParameter = this.ReadValue();
                    }
                    break;

                case "FallbackValue":
                    this.ParseEquals(block);
                    if (description.FallbackValue != null)
                        MvxBindingTrace.Warning("Overwriting existing FallbackValue");
                    description.FallbackValue = this.ReadValue();
                    break;

                case "Mode":
                    this.ParseEquals(block);
                    //if (description.Mode != MvxBindingMode.Default)
                    //{
                    //    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Mode specified multiple times in binding in {0} - for readability either use <,>,<1,<> or use (Mode=...) - not both", FullText);
                    //}
                    description.Mode = this.ReadBindingMode();
                    break;

                default:
                    this.ParseNonKeywordBlockInto(description, block);
                    break;
            }
        }

        protected virtual void HandleEmptyBlock(MvxSerializableBindingDescription description)
        {
            // default implementation doesn't do any special handling on an empty block
        }

        protected virtual void ParseNonKeywordBlockInto(MvxSerializableBindingDescription description, string block)
        {
            if (!this.IsComplete && this.CurrentChar == '(')
            {
                this.ParseFunctionStyleBlockInto(description, block);
            }
            else
            {
                this.ThrowExceptionIfPathAlreadyDefined(description);
                description.Path = block;
            }
        }

        protected virtual void ParseFunctionStyleBlockInto(MvxSerializableBindingDescription description, string block)
        {
            description.Converter = block;
            this.MoveNext();
            if (this.IsComplete)
                throw new MvxException("Unterminated () pair for converter {0}", block);

            this.ParseChildBindingDescriptionInto(description);
            this.SkipWhitespace();
            switch (this.CurrentChar)
            {
                case ')':
                    this.MoveNext();
                    break;

                case ',':
                    this.MoveNext();
                    this.ReadConverterParameterAndClosingBracket(description);
                    break;

                default:
                    throw new MvxException("Unexpected character {0} while parsing () contents", this.CurrentChar);
            }
        }

        protected void ReadConverterParameterAndClosingBracket(MvxSerializableBindingDescription description)
        {
            this.SkipWhitespace();
            description.ConverterParameter = this.ReadValue();
            this.SkipWhitespace();
            if (this.CurrentChar != ')')
                throw new MvxException("Unterminated () pair for converter {0}");
            this.MoveNext();
        }

        protected void ParseChildBindingDescriptionInto(MvxSerializableBindingDescription description,
                                                        ParentIsLookingForComma parentIsLookingForComma =
                                                            ParentIsLookingForComma.ParentIsLookingForComma)
        {
            this.SkipWhitespace();
            description.Function = "Single";
            description.Sources = new[]
                {
                    this.ParseBindingDescription(parentIsLookingForComma)
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
                    this.CurrentIndex, this.FullText);
            }
        }

        protected override MvxSerializableBindingDescription ParseBindingDescription()
        {
            return this.ParseBindingDescription(ParentIsLookingForComma.ParentIsNotLookingForComma);
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
            this.SkipWhitespace();

            while (true)
            {
                this.ParseNextBindingDescriptionOptionInto(description);

                this.SkipWhitespace();
                if (this.IsComplete)
                    return description;

                switch (this.CurrentChar)
                {
                    case ',':
                        if (parentIsLookingForComma == ParentIsLookingForComma.ParentIsLookingForComma)
                            return description;

                        this.MoveNext();
                        break;

                    case ';':
                    case ')':
                        return description;

                    default:
                        if (this.DetectOperator())
                            this.ParseOperatorWithLeftHand(description);
                        else
                            throw new MvxException(
                                "Unexpected character {0} at position {1} in {2} - expected string-end, ',' or ';'",
                                this.CurrentChar,
                                this.CurrentIndex,
                                this.FullText);
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