// MvxTibetBindingParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Parse.Binding.Tibet
{
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Binding.Parse.Binding.Swiss;
    using MvvmCross.Platform.Exceptions;

    public class MvxTibetBindingParser
        : MvxSwissBindingParser
    {
        public static readonly object LiteralNull = new object();

        private List<char> _terminatingCharacters;

        protected override IEnumerable<char> TerminatingCharacters()
        {
            return this._terminatingCharacters ??
                   (this._terminatingCharacters = base.TerminatingCharacters().Union(this.OperatorCharacters()).ToList());
        }

        private char[] OperatorCharacters()
        {
            return new char[] { '>', '<', '+', '-', '*', '/', '|', '&', '!', '=', '%' };
        }

        protected override void ParseNextBindingDescriptionOptionInto(MvxSerializableBindingDescription description)
        {
            if (this.IsComplete)
                return;

            object literal;
            if (this.TryReadValue(AllowNonQuotedText.DoNotAllow, out literal))
            {
                // for null, replace with LiteralNull
                literal = literal ?? LiteralNull;
                this.ThrowExceptionIfPathAlreadyDefined(description);
                description.Literal = literal;
                return;
            }

            base.ParseNextBindingDescriptionOptionInto(description);
        }

        protected override void ParseFunctionStyleBlockInto(MvxSerializableBindingDescription description, string block)
        {
            description.Function = block;
            this.MoveNext();
            if (this.IsComplete)
                throw new MvxException("Unterminated () pair for combiner {0}", block);

            var terminationFound = false;
            var sources = new List<MvxSerializableBindingDescription>();
            while (!terminationFound)
            {
                this.SkipWhitespace();
                sources.Add(this.ParseBindingDescription(ParentIsLookingForComma.ParentIsLookingForComma));
                this.SkipWhitespace();
                if (this.IsComplete)
                    throw new MvxException("Unterminated () while parsing combiner {0}", block);

                switch (this.CurrentChar)
                {
                    case ')':
                        this.MoveNext();
                        terminationFound = true;
                        break;

                    case ',':
                        this.MoveNext();
                        break;

                    default:
                        throw new MvxException("Unexpected character {0} while parsing () combiner contents for {1}", this.CurrentChar, block);
                }
            }

            description.Sources = sources.ToArray();
        }

        protected override MvxSerializableBindingDescription ParseOperatorWithLeftHand(MvxSerializableBindingDescription description)
        {
            // get the operator Combiner
            var twoCharacterOperatorString = this.SafePeekString(2);

            // TODO - I guess this should be done by dictionaries
            string combinerName = null;
            uint moveFowards = 0;
            switch (twoCharacterOperatorString)
            {
                case "!=":
                    combinerName = "NotEqualTo";
                    moveFowards = 2;
                    break;

                case ">=":
                    combinerName = "GreaterThanOrEqualTo";
                    moveFowards = 2;
                    break;

                case "<=":
                    combinerName = "LessThanOrEqualTo";
                    moveFowards = 2;
                    break;

                case "==":
                    combinerName = "EqualTo";
                    moveFowards = 2;
                    break;

                case "&&":
                    combinerName = "And";
                    moveFowards = 2;
                    break;

                case "||":
                    combinerName = "Or";
                    moveFowards = 2;
                    break;
            }

            // TODO - I guess this should be done by dictionaries
            if (combinerName == null)
            {
                switch (this.CurrentChar)
                {
                    case '>':
                        combinerName = "GreaterThan";
                        moveFowards = 1;
                        break;

                    case '<':
                        combinerName = "LessThan";
                        moveFowards = 1;
                        break;

                    case '+':
                        combinerName = "Add";
                        moveFowards = 1;
                        break;

                    case '-':
                        combinerName = "Subtract";
                        moveFowards = 1;
                        break;

                    case '*':
                        combinerName = "Multiply";
                        moveFowards = 1;
                        break;

                    case '/':
                        combinerName = "Divide";
                        moveFowards = 1;
                        break;

                    case '%':
                        combinerName = "Modulus";
                        moveFowards = 1;
                        break;
                }
            }

            if (combinerName == null)
                throw new MvxException("Unexpected operator starting with {0}", this.CurrentChar);

            this.MoveNext(moveFowards);

            // now create the operator Combiner
            var child = new MvxSerializableBindingDescription()
            {
                Path = description.Path,
                Literal = description.Literal,
                Sources = description.Sources,
                Function = description.Function,
                Converter = description.Converter,
                FallbackValue = description.FallbackValue,
                ConverterParameter = description.ConverterParameter,
                Mode = description.Mode
            };

            description.Converter = null;
            description.ConverterParameter = null;
            description.FallbackValue = null;
            description.Path = null;
            description.Mode = MvxBindingMode.Default;
            description.Literal = null;
            description.Function = combinerName;
            description.Sources = new List<MvxSerializableBindingDescription>()
                {
                    child,
                    this.ParseBindingDescription(ParentIsLookingForComma.ParentIsLookingForComma)
                };

            return description;
        }

        protected override bool DetectOperator()
        {
            return this.OperatorCharacters().Contains(this.CurrentChar);
        }

        protected override void HandleEmptyBlock(MvxSerializableBindingDescription description)
        {
            if (this.IsComplete)
                return;

            if (this.CurrentChar == '(')
            {
                this.MoveNext();
                this.ParseChildBindingDescriptionInto(description, ParentIsLookingForComma.ParentIsNotLookingForComma);

                this.SkipWhitespace();
                if (this.IsComplete || this.CurrentChar != ')')
                    throw new MvxException("Unterminated () pair");
                this.MoveNext();
                this.SkipWhitespace();
            }
        }
    }
}