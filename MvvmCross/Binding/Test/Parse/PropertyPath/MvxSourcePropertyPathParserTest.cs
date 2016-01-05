// MvxSourcePropertyPathParserTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Test.Parse.PropertyPath
{
    using System.Collections.Generic;

    using MvvmCross.Binding.Parse.PropertyPath;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    using NUnit.Framework;

    [TestFixture]
    public class MvxSourcePropertyPathParserTest
    {
        [Test]
        public void TestTokeniser_OnEmpty()
        {
            foreach (var test in new[] { null, string.Empty, ".", "\t", " .\r\n" })
            {
                var result = this.Tokenise(test);
                Assert.AreEqual(1, result.Count);
                Assert.IsInstanceOf<MvxEmptyPropertyToken>(result[0]);
            }
        }

        [Test]
        public void TestTokeniser_OnWhitespace()
        {
            var result = this.Tokenise(" \t\r \n ");
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOf<MvxEmptyPropertyToken>(result[0]);
        }

        [Test]
        public void TestTokeniser_OnSimpleProperty()
        {
            var text = "Hello";
            var result = this.Tokenise(text);
            Assert.AreEqual(1, result.Count);
            AssertIsSimplePropertyToken(result[0], text);

            var result2 = this.Tokenise(this.AddWhitespace(text));
            Assert.AreEqual(1, result2.Count);
            AssertIsSimplePropertyToken(result2[0], text);
        }

        [Test]
        public void TestTokeniser_OnChainedSimpleProperty()
        {
            var text = "Hello.World.Good.Morning.Foo.Bar";
            var split = text.Split('.');

            var result = this.Tokenise(text);
            Assert.AreEqual(6, result.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = this.Tokenise(this.AddWhitespace(text));
            Assert.AreEqual(6, result2.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Test]
        public void TestTokeniser_OnIntegerPropertyIndexer()
        {
            var toTest = new[] { 0, 1, 123, int.MaxValue };
            foreach (var u in toTest)
            {
                var text = "[" + u + "]";

                var result = this.Tokenise(text);
                Assert.AreEqual(1, result.Count);
                AssertIsIndexerPropertyToken<int, MvxIntegerIndexerPropertyToken>(result[0], u);

                var result2 = this.Tokenise(this.AddWhitespace(text));
                Assert.AreEqual(1, result2.Count);
                AssertIsIndexerPropertyToken<int, MvxIntegerIndexerPropertyToken>(result2[0], u);
            }
        }

        [Test]
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

                    var result = this.Tokenise(text);
                    Assert.AreEqual(1, result.Count);
                    AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result[0], s);

                    var result2 = this.Tokenise(this.AddWhitespace(text));
                    Assert.AreEqual(1, result2.Count);
                    AssertIsIndexerPropertyToken<string, MvxStringIndexerPropertyToken>(result2[0], s);
                }
            }
        }

        [Test]
        public void TestTokeniser_OnChainedSimplePropertyWithUnderscore()
        {
            var text = "Hello_World.Good.Mor_ning.Foo.Bar";
            var split = text.Split('.');

            var result = this.Tokenise(text);
            Assert.AreEqual(5, result.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = this.Tokenise(this.AddWhitespace(text));
            Assert.AreEqual(5, result2.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Test]
        public void TestTokeniser_OnChainedSimplePropertyWithInitialUnderscore()
        {
            var text = "_Hello_World.Good._Mor_ning.Foo._Bar";
            var split = text.Split('.');

            var result = this.Tokenise(text);
            Assert.AreEqual(5, result.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result[i], split[i]);
            }

            var result2 = this.Tokenise(this.AddWhitespace(text));
            Assert.AreEqual(5, result2.Count);
            for (var i = 0; i < split.Length; i++)
            {
                AssertIsSimplePropertyToken(result2[i], split[i]);
            }
        }

        [Test]
        public void TestTokeniser_SmokeTest()
        {
            var testString = "I [ 'Like - it hot -' ] .\tNew. York[1972]. In .Summer [\"\"]";
            var result = this.Tokenise(testString);
            Assert.AreEqual(8, result.Count);
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
            Assert.IsInstanceOf<MvxPropertyNamePropertyToken>(token);
            Assert.AreEqual(text, ((MvxPropertyNamePropertyToken)token).PropertyName);
        }

        private static void AssertIsIndexerPropertyToken<T, TSpecific>(MvxPropertyToken token, T value)
        {
            Assert.IsInstanceOf<MvxIndexerPropertyToken<T>>(token);
            Assert.IsInstanceOf<TSpecific>(token);
            Assert.AreEqual(value, ((MvxIndexerPropertyToken<T>)token).Key);
        }

        private IList<MvxPropertyToken> Tokenise(string text)
        {
            var tokeniser = new MvxSourcePropertyPathParser();
            return tokeniser.Parse(text);
        }
    }
}