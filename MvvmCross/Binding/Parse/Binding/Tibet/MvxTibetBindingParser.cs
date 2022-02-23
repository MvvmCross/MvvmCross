// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Parse.Binding.Swiss;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Parse.Binding.Tibet
{
    public class MvxTibetBindingParser
        : MvxSwissBindingParser
    {
        public static readonly object LiteralNull = new object();

        public static readonly char[] OperatorCharacters = { '>', '<', '+', '-', '*', '/', '|', '&', '!', '=', '%', '^' };

        private static Dictionary<string, string> TwoCharacterOperatorCombinerNames => new Dictionary<string, string>
        {
            { "!=", "NotEqualTo" },
            { ">=", "GreaterThanOrEqualTo" },
            { "<=", "LessThanOrEqualTo" },
            { "==", "EqualTo" },
            { "&&", "And" },
            { "||", "Or" }
        };

        private static Dictionary<char, string> SingleCharacterOperatorCombinerNames => new Dictionary<char, string>
        {
            { '>', "GreaterThan" },
            { '<', "LessThan" },
            { '+', "Add" },
            { '-', "Subtract" },
            { '*', "Multiply" },
            { '/', "Divide" },
            { '%', "Modulus" },
            { '!', "Inverted" },
            { '^', "XOr" }
        };

        private char[] _terminatingCharacters;

        protected override IEnumerable<char> TerminatingCharacters() =>
            _terminatingCharacters ?? (_terminatingCharacters = base.TerminatingCharacters().Union(OperatorCharacters).ToArray());

        protected override void ParseNextBindingDescriptionOptionInto(MvxSerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            object literal;
            if (TryReadValue(AllowNonQuotedText.DoNotAllow, out literal))
            {
                // for null, replace with LiteralNull
                literal = literal ?? LiteralNull;
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

        private Tuple<uint, string> ParseTwoCharacterOperator()
        {
            uint moveFowards = 0;
            var twoCharacterOperatorString = SafePeekString(2);
            var gotCombinerName = TwoCharacterOperatorCombinerNames.TryGetValue(twoCharacterOperatorString, out string combinerName);
            if (gotCombinerName)
                moveFowards = 2;

            return Tuple.Create(moveFowards, combinerName);
        }

        protected override MvxSerializableBindingDescription ParseOperatorWithLeftHand(MvxSerializableBindingDescription description)
        {
            // Parse the operator
            var parsed = ParseTwoCharacterOperator();
            var moveForwards = parsed.Item1;
            var combinerName = parsed.Item2;

            if (combinerName == null)
            {
                var gotCombinerName = SingleCharacterOperatorCombinerNames.TryGetValue(CurrentChar, out combinerName);
                if (gotCombinerName)
                    moveForwards = 1;
            }

            if (combinerName == null)
                throw new MvxException("Unexpected operator starting with {0}", CurrentChar);

            MoveNext(moveForwards);

            // now create the operator Combiner
            var child = new MvxSerializableBindingDescription
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
            description.Sources = new List<MvxSerializableBindingDescription>
            {
                child,
                ParseBindingDescription(ParentIsLookingForComma.ParentIsLookingForComma)
            };

            return description;
        }

        protected override bool DetectOperator() => OperatorCharacters.Contains(CurrentChar);

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
