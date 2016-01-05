// MvxLanguageBindingParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Parse.Binding.Lang
{
    using MvvmCross.Platform.Exceptions;

    public class MvxLanguageBindingParser
        : MvxBindingParser
          , IMvxLanguageBindingParser
    {
        public MvxBindingMode DefaultBindingMode { get; set; }

        public string DefaultConverterName { get; set; }

        public string DefaultTextSourceName { get; set; }

        public MvxLanguageBindingParser()
        {
            this.DefaultConverterName = "Language";
            this.DefaultTextSourceName = "TextSource";
            this.DefaultBindingMode = MvxBindingMode.OneTime;
        }

        protected void ParseNextBindingDescriptionOptionInto(MvxSerializableBindingDescription description)
        {
            if (this.IsComplete)
                return;

            var block = this.ReadTextUntilNonQuotedOccurrenceOfAnyOf('=', ',', ';');
            block = block.Trim();
            if (string.IsNullOrEmpty(block))
            {
                return;
            }

            switch (block)
            {
                case "Source":
                    this.ParseEquals(block);
                    var sourceName = this.ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
                    description.Path = sourceName;
                    break;

                case "Converter":
                    this.ParseEquals(block);
                    description.Converter = this.ReadValidCSharpName();
                    break;

                case "Key":
                    this.ParseEquals(block);
                    description.ConverterParameter = this.ReadValue();
                    break;

                case "FallbackValue":
                    this.ParseEquals(block);
                    description.FallbackValue = this.ReadValue();
                    break;

                default:
                    if (description.ConverterParameter != null)
                    {
                        throw new MvxException(
                            "Problem parsing Language Binding near '{0}', Key set to '{1}', position {2} in {3}",
                            block, description.ConverterParameter, this.CurrentIndex, this.FullText);
                    }

                    block = this.UnquoteBlockIfNecessary(block);

                    description.ConverterParameter = block;
                    break;
            }
        }

        private string UnquoteBlockIfNecessary(string block)
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
                Converter = this.DefaultConverterName,
                Path = this.DefaultTextSourceName,
                Mode = this.DefaultBindingMode
            };

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
                        this.MoveNext();
                        break;

                    case ';':
                        return description;

                    default:
                        throw new MvxException(
                            "Unexpected character {0} at position {1} in {2} - expected string-end, ',' or ';'",
                            this.CurrentChar,
                            this.CurrentIndex,
                            this.FullText);
                }
            }
        }
    }
}