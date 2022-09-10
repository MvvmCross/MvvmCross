// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using Xunit;

namespace MvvmCross.UnitTest.Base
{

    public class MvxParserTest
    {
        public class BaseToken
        {
        }

        public class Parser : MvxParser
        {
            public void CallReset(string toParse)
            {
                Reset(toParse);
            }

            public string GetFullText()
            {
                return FullText;
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

            public void CallMoveNext(uint increment = 1)
            {
                MoveNext(increment);
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

            public object CallReadValue()
            {
                return ReadValue();
            }

            public object CallReadEnumerationValue(Type enumerationType, bool ignoreCase = true)
            {
                return ReadEnumerationValue(enumerationType, ignoreCase);
            }

            public string CallReadValidCSharpName()
            {
                return ReadValidCSharpName();
            }

            public string CallReadTextUntilWhitespaceOr(params char[] items)
            {
                return ReadTextUntilWhitespaceOr(items);
            }

            public bool CallIsValidFirstCharacterOfCSharpName(char character)
            {
                return IsValidFirstCharacterOfCSharpName(character);
            }


        }

        [Fact]
        public void Test_Reset_Clears_And_Sets_All_Properties()
        {
            var tokeniser = new Parser();
            Assert.Null(tokeniser.GetFullText());
            Assert.Equal(0, tokeniser.GetCurrentIndex());
            var testString1 = @"1 test";
            tokeniser.CallReset(testString1);
            Assert.Equal(testString1, tokeniser.GetFullText());
            Assert.Equal(0, tokeniser.GetCurrentIndex());
            Assert.Equal(testString1[0], tokeniser.GetCurrentChar());
            var testString2 = @"2 test";
            tokeniser.CallReset(testString2);
            Assert.Equal(testString2, tokeniser.GetFullText());
            Assert.Equal(0, tokeniser.GetCurrentIndex());
            Assert.Equal(testString2[0], tokeniser.GetCurrentChar());
        }

        [Fact]
        public void Test_CallReadQuotedString_Reads_Strings()
        {
            var testStrings = new Dictionary<string, string>
                {
                    { "''", "" },
                    { "\"Hello \\\"\\\'\"", "Hello \"\'" },
                    { "\"Hello\"", "Hello" },
                    { "'Hello'", "Hello" },
                    { "'Hello World'", "Hello World" },
                    { "'Hello \\r'", "Hello \r" },
                    { "'Hello \\u1234'", "Hello \u1234" },
                    { "'Hello \\U00001234'", "Hello \U00001234" }
                };

            foreach (var testString in testStrings)
            {
                var tokeniser = new Parser();
                tokeniser.CallReset(testString.Key);
                var result = tokeniser.CallReadQuotedString();
                Assert.Equal(testString.Value, result);
                Assert.True(tokeniser.GetIsComplete());
            }
        }

        [Fact]
        public void Test_CallReadUnsignedInteger_Reads_Numbers()
        {
            var testStrings = new Dictionary<string, uint>
                {
                    { "2", 2 },
                    { "1234", 1234 },
                    { uint.MaxValue.ToString(), uint.MaxValue },
                    { uint.MinValue.ToString(), uint.MinValue }
                };

            foreach (var testString in testStrings)
            {
                var tokeniser = new Parser();
                tokeniser.CallReset(testString.Key);
                var result = tokeniser.CallReadUnsignedInteger();
                Assert.Equal(testString.Value, result);
                Assert.True(tokeniser.GetIsComplete());
            }
        }

        [Fact]
        public void Test_ReadValue_Reads_Quoted_Strings()
        {
            var tests = new Dictionary<string, object>
                {
                    { "\'Foo Bar\'", "Foo Bar" },
                    { "\"Foo Bar\"", "Foo Bar" },
                    { "\'Foo \\\' \\\" Bar\'", "Foo \' \" Bar" },
                    { "\"Foo \\\' \\\" Bar\"", "Foo \' \" Bar" },
                };
            DoReadValueTests(tests);
        }

        private void DoReadValueTests(Dictionary<string, object> tests)
        {
            foreach (var t in tests)
                DoReadValueTest(t.Key, t.Value);
        }

        private void DoReadValueTest(string input, object expectedOutput)
        {
            var parser = new Parser();
            parser.CallReset(input);
            var output = parser.CallReadValue();
            Assert.Equal(expectedOutput, output);
            Assert.True(parser.GetIsComplete());
        }

        [Fact]
        public void Test_ReadValue_Reads_Booleans()
        {
            var tests = new Dictionary<string, object>
                {
                    { "TruE", true },
                    { "true", true },
                    { "TRUE", true },
                    { "faLsE", false },
                    { "FALSE", false },
                    { "false", false }
                };
            DoReadValueTests(tests);
        }

        [Fact]
        public void Test_ReadTextUntilWhitespaceOr_ReadsText()
        {
            var tests = new string[] { "fred;", "fred life;", "fred@test;", "fred", "fred\tlife;", "fred\n" };
            foreach (var test in tests)
            {
                var parser = new Parser();
                parser.CallReset(test);
                var output = parser.CallReadTextUntilWhitespaceOr('@', ';', ')');
                Assert.Equal("fred", output);
            }
        }

        [Fact]
        public void Test_ReadValue_Reads_Integers()
        {
            var tests = new[] { 0, 1, 2, 3, -123, -1, long.MinValue, long.MaxValue };
            var dict = tests.ToDictionary(x => x.ToString(CultureInfo.InvariantCulture),
                                          x => (object)x);
            DoReadValueTests(dict);
        }

        [Fact]
        public void Test_ReadValue_Reads_Doubles()
        {
            // note - we don't run any tests on things like double.MinValue - those would fail due to rounding errors.
            var tests = new[] { 0.001, 1.123, 2.2343, -123.1232, -1.2323, -99999.93454, 9999.343455 };
            var dict = tests.ToDictionary(x => x.ToString(CultureInfo.InvariantCulture),
                                          x => (object)x);
            DoReadValueTests(dict);
        }

        public enum MyEnum
        {
            Value1,
            Value2,
            MyEnumValue3Foo
        }

        private void DoReadEnumerationTest(Type enumerationType, string input, object expectedOutput)
        {
            var parser = new Parser();
            parser.CallReset(input);
            var output = parser.CallReadEnumerationValue(enumerationType, true /* ignoreCase */);
            Assert.Equal(expectedOutput, output);
            Assert.True(parser.GetIsComplete());
        }

        [Fact]
        public void Test_ReadEnumeration_Reads_Enumerations()
        {
            foreach (var value in Enum.GetValues(typeof(MyEnum)))
            {
                DoReadEnumerationTest(typeof(MyEnum), value.ToString(), value);
                DoReadEnumerationTest(typeof(MyEnum), value.ToString().ToUpper(), value);
                DoReadEnumerationTest(typeof(MyEnum), value.ToString().ToLower(), value);
            }
        }

        [Fact]
        public void Test_ReadValidCSharpName_Succeeds()
        {
            var names = new[]
                {
                    @"Foo",
                    @"foo",
                    @"A",
                    @"a",
                    @"_x",
                    @"x_x",
                    @"_2",
                    @"A5",
                    @"B_5_d",
                    @"B5_d_s_",
                };

            foreach (var name in names)
            {
                var parser = new Parser();
                parser.CallReset(name);
                var result = parser.CallReadValidCSharpName();
                Assert.Equal(name, result);
                Assert.True(parser.GetIsComplete());

                var parser2 = new Parser();
                parser2.CallReset("\t " + name + " \r \t");
                var result2 = parser2.CallReadValidCSharpName();
                Assert.Equal(name, result2);
                Assert.False(parser2.GetIsComplete());
            }
        }

        [Fact]
        public void Test_ReadValidCSharpName_Fails()
        {
            var names = new[]
                {
                    "$Foo",
                    "+foo",
                    "-A",
                    "\"A\"",
                };

            foreach (var name in names)
            {
                var parser = new Parser();
                parser.CallReset(name);
                var exceptionThrown = false;
                try
                {
                    var result = parser.CallReadValidCSharpName();
                }
                catch (MvxException)
                {
                    exceptionThrown = true;
                }
                Assert.True(exceptionThrown);
            }
        }

        [Fact]
        public void Reset_Resets_FullText_And_Index()
        {
            var parser = new Parser();
            parser.CallReset("derp");

            Assert.Equal(0, parser.GetCurrentIndex());
            Assert.Equal("derp", parser.GetFullText());
        }

        [Theory]
        [InlineData('a')]
        [InlineData('b')]
        [InlineData('c')]
        [InlineData('d')]
        [InlineData('e')]
        [InlineData('f')]
        [InlineData('g')]
        [InlineData('_')]
        [InlineData('Z')]
        [InlineData('Æ')]
        [InlineData('ø')]
        [InlineData('ä')]
        [InlineData('µ')]
        public void TestIsValidFirstCharacterOfCSharpName_OK(char character)
        {
            var parser = new Parser();
            var result = parser.CallIsValidFirstCharacterOfCSharpName(character);
            Assert.True(result);
        }

        [Theory]
        [InlineData('1')]
        [InlineData(' ')]
        [InlineData('-')]
        [InlineData('\\')]
        [InlineData('^')]
        [InlineData('%')]
        [InlineData('$')]
        [InlineData('#')]
        [InlineData('@')]
        [InlineData('!')]
        [InlineData('&')]
        [InlineData('*')]
        [InlineData('(')]
        [InlineData(')')]
        [InlineData('+')]
        [InlineData('=')]
        public void TestIsValidFirstCharacterOfCSharpName_Fail(char character)
        {
            var parser = new Parser();
            var result = parser.CallIsValidFirstCharacterOfCSharpName(character);
            Assert.False(result);
        }
    }
}
