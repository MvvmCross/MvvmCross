using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Binding.Parser;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.Parser
{
    [TestFixture]
    public class MvxBaseTokeniserTest
    {
        public class BaseToken
        {            
        }

        public class Tokeniser : MvxBaseTokeniser<BaseToken>
        {
            public void CallReset(string toParse)
            {
                Reset(toParse);
            }
            public string GetFullText()
            {
                return FullText;
            }
            public List<BaseToken> GetCurrentTokens()
            {
                return CurrentTokens;
            }
            public bool GetIsComplete()
            {
                return IsComplete;
            }
            public int GetCurrentIndex()
            {
                return CurrentIndex;
            }
            public char GetCurrentChar()
            {
                return CurrentChar;
            }
            public void CallMoveNext()
            {
                MoveNext();
            }
            public string CallReadQuotedString()
            {
                return ReadQuotedString();
            }
            public uint CallReadUnsignedInteger()
            {
                return ReadUnsignedInteger();
            }
            public char CallReadEscapedCharacter()
            {
                return ReadEscapedCharacter();
            }
            public void CallSkipWhitespace()
            {
                SkipWhitespace();
            }
            public void CallSkipWhitespaceAndCharacters(Dictionary<char, bool> toSkip)
            {
                SkipWhitespaceAndCharacters(toSkip);
            }
            public void CallSkipWhitespaceAndCharacters(IEnumerable<char> toSkip)
            {
                SkipWhitespaceAndCharacters(toSkip);
            }
        }

        [Test]
        public void Test_Reset_Clears_And_Sets_All_Properties()
        {
            var tokeniser = new Tokeniser();
            Assert.IsNull(tokeniser.GetFullText());
            Assert.AreEqual(0, tokeniser.GetCurrentIndex());
            Assert.IsNull(tokeniser.GetCurrentTokens());
            var testString1 = @"1 test";
            tokeniser.CallReset(testString1);
            Assert.AreEqual(testString1,tokeniser.GetFullText());
            Assert.AreEqual(0, tokeniser.GetCurrentIndex());
            Assert.AreEqual(testString1[0], tokeniser.GetCurrentChar());
            Assert.AreEqual(0, tokeniser.GetCurrentTokens().Count);
            tokeniser.GetCurrentTokens().Add(new BaseToken());
            Assert.AreEqual(1, tokeniser.GetCurrentTokens().Count);
            var testString2 = @"2 test";
            tokeniser.CallReset(testString2);
            Assert.AreEqual(testString2, tokeniser.GetFullText());
            Assert.AreEqual(0, tokeniser.GetCurrentIndex());
            Assert.AreEqual(testString2[0], tokeniser.GetCurrentChar());
            Assert.AreEqual(0, tokeniser.GetCurrentTokens().Count);
        }

        [Test]
        public void Test_CallReadQuotedString_Reads_Strings()
        {
            var testStrings = new Dictionary<string, string>()
                {
                    { "''", "" },
                    { "\"Hello \\\"\\\'\"", "Hello \"\'" },
                    { "\"Hello\"", "Hello" },
                    { "'Hello'", "Hello" },
                    { "'Hello World'", "Hello World" },
                    { "'Hello \\r'", "Hello \r" },
                    { "'Hello \\u1234'", "Hello \u1234" },
                    { "'Hello \\U00001234'", "Hello \U00001234" },
                };

            foreach (var testString in testStrings)
            {
                var tokeniser = new Tokeniser();
                tokeniser.CallReset(testString.Key);
                var result = tokeniser.CallReadQuotedString();
                Assert.AreEqual(testString.Value, result);
                Assert.IsTrue(tokeniser.GetIsComplete());                
            }
        }

        [Test]
        public void Test_CallReadUnsignedInteger_Reads_Numbers()
        {
            var testStrings = new Dictionary<string, uint>()
                {
                    { "2", 2 },
                    { "1234", 1234 },
                    { UInt32.MaxValue.ToString(), UInt32.MaxValue },
                    { UInt32.MinValue.ToString(), UInt32.MinValue },
                };

            foreach (var testString in testStrings)
            {
                var tokeniser = new Tokeniser();
                tokeniser.CallReset(testString.Key);
                var result = tokeniser.CallReadUnsignedInteger();
                Assert.AreEqual(testString.Value, result);
                Assert.IsTrue(tokeniser.GetIsComplete());                
            }
        }        
    }
}
