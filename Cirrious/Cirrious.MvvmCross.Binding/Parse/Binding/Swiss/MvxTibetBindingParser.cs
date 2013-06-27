// MvxTibetBindingParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Binding.Parse.Binding.Swiss
{
    public class MvxTibetBindingParser
        : MvxSwissBindingParser
    {
        private List<char> _terminatingCharacters;
 
        protected override IEnumerable<char> TerminatingCharacters()
        {
            if (_terminatingCharacters == null)
            {
                _terminatingCharacters =
                    base.TerminatingCharacters().Union(OperatorCharacters()).ToList();
            }

            return _terminatingCharacters;
        }

        private char[] OperatorCharacters()
        {
            return new char[] {'>', '<', '+', '-', '*', '/', '|', '&', '!', '=', '%'};
        }

        protected override void ParseNextBindingDescriptionOptionInto(MvxSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            object literal;
            if (TryReadValue(AllowNonQuotedText.DoNotAllow, out literal))
            {
                ThrowExceptionIfPathAlreadyDefined(description);
                description.Literal = literal;
                return;
            }

            base.ParseNextBindingDescriptionOptionInto(description);
        }

        protected override void ParseFunctionStyleBlockInto(MvxSerializableBindingDescription description, string block)
        {
            description.Function = block;
            MoveNext();
            if (IsComplete)
                throw new MvxException("Unterminated () pair for combiner {0}", block);

            var terminationFound = false;
            var sources = new List<MvxSerializableBindingDescription>();
            while (!terminationFound)
            {
                SkipWhitespace();
                sources.Add(ParseBindingDescription(ParentIsLookingForComma.ParentIsLookingForComma));
                SkipWhitespace();
                if (IsComplete)
                    throw new MvxException("Unterminated () while parsing combiner {0}", block);
                    
                switch (CurrentChar)
                {
                    case ')':
                        MoveNext();
                        terminationFound = true;
                        break;
                    case ',':
                        MoveNext();
                        break;
                    default:
                        throw new MvxException("Unexpected character {0} while parsing () combiner contents for {1}", CurrentChar, block);
                }
            }

            description.Sources = sources.ToArray();
        }

        protected override MvxSerializableBindingDescription ParseOperatorWithLeftHand(MvxSerializableBindingDescription description)
        {
            // get the operator Combiner
            var twoCharacterOperatorString = SafePeekString(2);

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
                switch (CurrentChar)
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
                throw new MvxException("Unexpected operator starting with {0}", CurrentChar);

            MoveNext(moveFowards);

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
                    ParseBindingDescription(ParentIsLookingForComma.ParentIsLookingForComma)
                };

            return description;
        }

        protected override bool DetectOperator()
        {
            return OperatorCharacters().Contains(CurrentChar);
        }

        protected override void HandleEmptyBlock(MvxSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            if (CurrentChar == '(')
            {
                MoveNext();
                ParseChildBindingDescriptionInto(description, ParentIsLookingForComma.ParentIsNotLookingForComma);

                SkipWhitespace();
                if (IsComplete || CurrentChar != ')')
                    throw new MvxException("Unterminated () pair");
                MoveNext();
                SkipWhitespace();
            }
        }
    }
}