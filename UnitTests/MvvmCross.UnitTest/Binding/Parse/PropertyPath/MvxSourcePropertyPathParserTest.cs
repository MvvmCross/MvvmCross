// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Parse.PropertyPath;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.UnitTest.Binding.Parse.PropertyPath
{
    public class MvxSourcePropertyPathParserTest
    {
        [Test]
        public async Task TestTokeniser_OnEmpty()
        {
            foreach (var test in new[] { null, string.Empty, ".", "\t", " .\r\n" })
            {
                var result = Tokenise(test);
                await Assert.That(result).HasSingleItem();
                await Assert.That(result[0]).IsTypeOf<MvxEmptyPropertyToken>();
            }
        }

        [Test]
        public async Task TestTokeniser_OnWhitespace()
        {
            var result = Tokenise(" \t\r \n ");
            await Assert.That(result).HasSingleItem();
            await Assert.That(result[0]).IsTypeOf<MvxEmptyPropertyToken>();
        }

        [Test]
        public async Task TestTokeniser_OnSimpleProperty()
        {
            var text = "Hello";
            var result = Tokenise(text);
            await Assert.That(result).HasSingleItem();
            await AssertIsSimplePropertyToken(result[0], text);

            var result2 = Tokenise(AddWhitespace(text));
            await Assert.That(result2).HasSingleItem();
            await AssertIsSimplePropertyToken(result2[0], text);
        }

        [Test]
        public async Task TestTokeniser_OnChainedSimpleProperty()
        {
            var text = "Hello.World.Good.Morning.Foo.Bar";
            var split = text.Split('.');

            var result = Tokenise(text);
            await Assert.That(result.Count).IsEqualTo(6);
            for (var i = 0; i < split.Length; i++)
            {
                await AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = Tokenise(AddWhitespace(text));
            await Assert.That(result2.Count).IsEqualTo(6);
            for (var i = 0; i < split.Length; i++)
            {
                await AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Test]
        public async Task TestTokeniser_OnIntegerPropertyIndexer()
        {
            var toTest = new[] { 0, 1, 123, int.MaxValue };
            foreach (var u in toTest)
            {
                var text = "[" + u + "]";

                var result = Tokenise(text);
                await Assert.That(result).HasSingleItem();
                await AssertIsIndexerPropertyToken<int, MvxIntegerIndexerPropertyToken>(result[0], u);

                var result2 = Tokenise(AddWhitespace(text));
                await Assert.That(result2).HasSingleItem();
                await AssertIsIndexerPropertyToken<int, MvxIntegerIndexerPropertyToken>(result2[0], u);
            }
        }

        [Test]
        public async Task TestTokeniser_OnStringPropertyIndexer()
        {
            var toTest = new[] { "One", "", "Foo\\Bar", "Hello\r\n" };
            var quotes = new[] { "\"", "'" };
            foreach (var s in toTest)
            {
                foreach (var quoteChar in quotes)
                {
                    var text = "[" + quoteChar + s + quoteChar + "]";
                    text = text.Replace("\\", "\\\\");

                    var result = Tokenise(text);
                    await Assert.That(result).HasSingleItem();
                    await AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result[0], s);

                    var result2 = Tokenise(AddWhitespace(text));
                    await Assert.That(result2).HasSingleItem();
                    await AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result2[0], s);
                }
            }
        }

        [Test]
        public async Task TestTokeniser_OnChainedSimplePropertyWithUnderscore()
        {
            var text = "Hello_World.Good.Mor_ning.Foo.Bar";
            var split = text.Split('.');

            var result = Tokenise(text);
            await Assert.That(result.Count).IsEqualTo(5);
            for (var i = 0; i < split.Length; i++)
            {
                await AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = Tokenise(AddWhitespace(text));
            await Assert.That(result2.Count).IsEqualTo(5);
            for (var i = 0; i < split.Length; i++)
            {
                await AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Test]
        public async Task TestTokeniser_OnChainedSimplePropertyWithInitialUnderscore()
        {
            var text = "_Hello_World.Good._Mor_ning.Foo._Bar";
            var split = text.Split('.');

            var result = Tokenise(text);
            await Assert.That(result.Count).IsEqualTo(5);
            for (var i = 0; i < split.Length; i++)
            {
                await AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = Tokenise(AddWhitespace(text));
            await Assert.That(result2.Count).IsEqualTo(5);
            for (var i = 0; i < split.Length; i++)
            {
                await AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Test]
        public async Task TestTokeniser_SmokeTest()
        {
            var testString = "I [ 'Like - it hot -' ] .\tNew. York[1972]. In .Summer [\"\"]";
            var result = Tokenise(testString);
            await Assert.That(result.Count).IsEqualTo(8);
            await AssertIsSimplePropertyToken(result[0], "I");
            await AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result[1], "Like - it hot -");
            await AssertIsSimplePropertyToken(result[2], "New");
            await AssertIsSimplePropertyToken(result[3], "York");
            await AssertIsIndexerPropertyToken<int, MvxIntegerIndexerPropertyToken>(result[4], 1972);
            await AssertIsSimplePropertyToken(result[5], "In");
            await AssertIsSimplePropertyToken(result[6], "Summer");
            await AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result[7], "");
        }

        private string AddWhitespace(string text)
        {
            var toReturn = text;
            toReturn = toReturn.Replace(".", "   .\t ");
            toReturn = toReturn.Replace("[", "   \r[\t ");
            toReturn = toReturn.Replace("]", "   ]\n ");
            return "\t" + toReturn + "  \r \n  \t ";
        }

        private static async Task AssertIsSimplePropertyToken(IMvxPropertyToken token, string text)
        {
            await Assert.That(token).IsAssignableTo<MvxPropertyNamePropertyToken>();
            await Assert.That(((MvxPropertyNamePropertyToken)token).PropertyName).IsEqualTo(text);
        }

        private static async Task AssertIsIndexerPropertyToken<T, TSpecific>(IMvxPropertyToken token, T value)
        {
            await Assert.That(token).IsAssignableTo<MvxIndexerPropertyToken<T>>();
            await Assert.That(token).IsAssignableTo<TSpecific>();
            await Assert.That(((MvxIndexerPropertyToken<T>)token).Key).IsEqualTo(value);
        }

        private IList<IMvxPropertyToken> Tokenise(string text)
        {
            var tokeniser = new MvxSourcePropertyPathParser();
            return tokeniser.Parse(text);
        }

        [Test]
        public async Task TestTokeniser_ReturnsParsedValueSimilarToOriginalValueForSimpleExpressions()
        {
            var random = new Random(123);
            for (int i = 0; i < 100_000; i++)
            {
                var originalExpression = CreateRandomExpression(random);
                var mvxPropertyTokens = Tokenise(originalExpression);

                var actual = string.Join<string>(".",
                    mvxPropertyTokens.Cast<MvxPropertyNamePropertyToken>().Select(t => t.PropertyName));

                await Assert.That(actual).IsEqualTo(originalExpression);
            }
        }

        private static string CreateRandomExpression(Random random)
        {
            return string.Join<string>(".",
                Enumerable.Repeat(0, random.Next() % 6 + 3).Select(_ =>
                {
                    var length = random.Next() % 6 + 3;
                    return new string(Enumerable.Repeat("qwertyuiopasdfghjklzxcvbnm", length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                }));
        }
    }
}
