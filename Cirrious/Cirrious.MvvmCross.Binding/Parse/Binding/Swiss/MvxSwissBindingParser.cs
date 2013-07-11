// MvxSwissBindingParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Binding.Parse.Binding.Swiss
{
    public class MvxSwissBindingParser
        : MvxBindingParser
    {
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
                    ThrowExceptionIfPathAlreadyDefined(description, block);
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
                    ParseEquals(block);
                    if (!string.IsNullOrEmpty(description.Converter))
                        MvxBindingTrace.Warning("Overwriting existing Converter with CommandParameter");
                    description.Converter = "CommandParameter";
                    description.ConverterParameter = ReadValue();
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
                    ThrowExceptionIfPathAlreadyDefined(description, block);
                    description.Path = block;
                    break;
            }
        }

        private void ThrowExceptionIfPathAlreadyDefined(MvxSerializableBindingDescription description, string block)
        {
            if (!string.IsNullOrEmpty(description.Path))
            {
                throw new MvxException(
                    "Make sure you are using ';' to separate multiple bindings. You cannot specify Path more than once - first Path '{0}', second Path '{1}', position {2} in {3}",
                    description.Path, block, CurrentIndex, FullText);
            }
        }

        protected override MvxSerializableBindingDescription ParseBindingDescription()
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
                        throw new MvxException(
                            "Unexpected character {0} at position {1} in {2} - expected string-end, ',' or ';'",
                            CurrentChar,
                            CurrentIndex,
                            FullText);
                }
            }
        }
    }
}