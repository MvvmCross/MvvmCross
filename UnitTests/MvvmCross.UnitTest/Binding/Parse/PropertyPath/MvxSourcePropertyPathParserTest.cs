// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Parse.PropertyPath;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using Xunit;

namespace MvvmCross.UnitTest.Binding.Parse.PropertyPath
{
    public class MvxSourcePropertyPathParserTest
    {
        [Fact]
        public void TestTokeniser_OnEmpty()
        {
            foreach (var test in new[] { null, string.Empty, ".", "\t", " .\r\n" })
            {
                var result = Tokenise(test);
                Assert.Equal(1, result.Count);
                Assert.IsType<MvxEmptyPropertyToken>(result[0]);
            }
        }

        [Fact]
        public void TestTokeniser_OnWhitespace()
        {
            var result = Tokenise(" \t\r \n ");
            Assert.Equal(1, result.Count);
            Assert.IsType<MvxEmptyPropertyToken>(result[0]);
        }

        [Fact]
        public void TestTokeniser_OnSimpleProperty()
        {
            var text = "Hello";
            var result = Tokenise(text);
            Assert.Equal(1, result.Count);
            AssertIsSimplePropertyToken(result[0], text);

            var result2 = Tokenise(AddWhitespace(text));
            Assert.Equal(1, result2.Count);
            AssertIsSimplePropertyToken(result2[0], text);
        }

        [Fact]
        public void TestTokeniser_OnChainedSimpleProperty()
        {
            var text = "Hello.World.Good.Morning.Foo.Bar";
            var split = text.Split('.');

            var result = Tokenise(text);
            Assert.Equal(6, result.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = Tokenise(AddWhitespace(text));
            Assert.Equal(6, result2.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Fact]
        public void TestTokeniser_OnIntegerPropertyIndexer()
        {
            var toTest = new[] { 0, 1, 123, int.MaxValue };
            foreach (var u in toTest)
            {
                var text = "[" + u + "]";

                var result = Tokenise(text);
                Assert.Equal(1, result.Count);
                AssertIsIndexerPropertyToken<int, MvxIntegerIndexerPropertyToken>(result[0], u);

                var result2 = Tokenise(AddWhitespace(text));
                Assert.Equal(1, result2.Count);
                AssertIsIndexerPropertyToken<int, MvxIntegerIndexerPropertyToken>(result2[0], u);
            }
        }

        [Fact]
        public void TestTokeniser_OnStringPropertyIndexer()
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
                    Assert.Equal(1, result.Count);
                    AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result[0], s);

                    var result2 = Tokenise(AddWhitespace(text));
                    Assert.Equal(1, result2.Count);
                    AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result2[0], s);
                }
            }
        }

        [Fact]
        public void TestTokeniser_OnChainedSimplePropertyWithUnderscore()
        {
            var text = "Hello_World.Good.Mor_ning.Foo.Bar";
            var split = text.Split('.');

            var result = Tokenise(text);
            Assert.Equal(5, result.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = Tokenise(AddWhitespace(text));
            Assert.Equal(5, result2.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Fact]
        public void TestTokeniser_OnChainedSimplePropertyWithInitialUnderscore()
        {
            var text = "_Hello_World.Good._Mor_ning.Foo._Bar";
            var split = text.Split('.');

            var result = Tokenise(text);
            Assert.Equal(5, result.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = Tokenise(AddWhitespace(text));
            Assert.Equal(5, result2.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Fact]
        public void TestTokeniser_SmokeTest()
        {
            var testString = "I [ 'Like - it hot -' ] .\tNew. York[1972]. In .Summer [\"\"]";
            var result = Tokenise(testString);
            Assert.Equal(8, result.Count);
            AssertIsSimplePropertyToken(result[0], "I");
            AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result[1], "Like - it hot -");
            AssertIsSimplePropertyToken(result[2], "New");
            AssertIsSimplePropertyToken(result[3], "York");
            AssertIsIndexerPropertyToken<int, MvxIntegerIndexerPropertyToken>(result[4], 1972);
            AssertIsSimplePropertyToken(result[5], "In");
            AssertIsSimplePropertyToken(result[6], "Summer");
            AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result[7], "");
        }

        private string AddWhitespace(string text)
        {
            var toReturn = text;
            toReturn = toReturn.Replace(".", "   .\t ");
            toReturn = toReturn.Replace("[", "   \r[\t ");
            toReturn = toReturn.Replace("]", "   ]\n ");
            return "\t" + toReturn + "  \r \n  \t ";
        }

        private static void AssertIsSimplePropertyToken(MvxPropertyToken token, string text)
        {
            Assert.IsAssignableFrom<MvxPropertyNamePropertyToken>(token);
            Assert.Equal(text, ((MvxPropertyNamePropertyToken)token).PropertyName);
        }

        private static void AssertIsIndexerPropertyToken<T, TSpecific>(MvxPropertyToken token, T value)
        {
            Assert.IsAssignableFrom<MvxIndexerPropertyToken<T>>(token);
            Assert.IsAssignableFrom<TSpecific>(token);
            Assert.Equal(value, ((MvxIndexerPropertyToken<T>)token).Key);
        }

        private IList<MvxPropertyToken> Tokenise(string text)
        {
            var tokeniser = new MvxSourcePropertyPathParser();
            return tokeniser.Parse(text);
        }

        [Fact]
        public void TestTokeniser_ReturnsParsedValueSimilarToOriginalValueForSimpleExpressions()
        {
            var random = new Random(123);
            for (int i = 0; i < 100_000; i++)
            {
                var originalExpression = CreateRandomExpression(random);
                var mvxPropertyTokens = Tokenise(originalExpression);

                var actual = string.Join<string>(".",
                    mvxPropertyTokens.Cast<MvxPropertyNamePropertyToken>().Select(t => t.PropertyName));

                Assert.Equal(originalExpression, actual);
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
