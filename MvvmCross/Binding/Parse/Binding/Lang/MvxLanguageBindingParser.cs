// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Parse.Binding.Lang
{
    public class MvxLanguageBindingParser
        : MvxBindingParser, IMvxLanguageBindingParser
    {
        public MvxBindingMode DefaultBindingMode { get; set; }

        public string DefaultConverterName { get; set; }

        public string DefaultTextSourceName { get; set; }

        public MvxLanguageBindingParser()
        {
            DefaultConverterName = "Language";
            DefaultTextSourceName = "TextSource";
            DefaultBindingMode = MvxBindingMode.OneTime;
        }

        protected void ParseNextBindingDescriptionOptionInto(MvxSerializableBindingDescription description)
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
                case "Source":
                    ParseEquals(block);
                    var sourceName = ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
                    description.Path = sourceName;
                    break;

                case "Converter":
                    ParseEquals(block);
                    description.Converter = ReadValidCSharpName();
                    break;

                case "Key":
                    ParseEquals(block);
                    description.ConverterParameter = ReadValue();
                    break;

                case "FallbackValue":
                    ParseEquals(block);
                    description.FallbackValue = ReadValue();
                    break;

                default:
                    if (description.ConverterParameter != null)
                    {
                        throw new MvxException(
                            "Problem parsing Language Binding near '{0}', Key set to '{1}', position {2} in {3}",
                            block, description.ConverterParameter, CurrentIndex, FullText);
                    }

                    block = UnquoteBlockIfNecessary(block);

                    description.ConverterParameter = block;
                    break;
            }
        }

        private static string UnquoteBlockIfNecessary(string block)
        {
            if (string.IsNullOrEmpty(block))
                return block;

            if (block.Length < 2)
                return block;

            if ((block.StartsWith("\'") && block.EndsWith("\'"))
                || (block.StartsWith("\"") && block.EndsWith("\"")))
                return block.Substring(1, block.Length - 2);

            return block;
        }

        protected override MvxSerializableBindingDescription ParseBindingDescription()
        {
            var description = new MvxSerializableBindingDescription
            {
                Converter = DefaultConverterName,
                Path = DefaultTextSourceName,
                Mode = DefaultBindingMode
            };

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
