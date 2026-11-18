// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvvmCross.Base;
using MvvmCross.Exceptions;

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

        [Test]
        public async Task Test_Reset_Clears_And_Sets_All_Properties()
        {
            var tokeniser = new Parser();
            await Assert.That(tokeniser.GetFullText()).IsNull();
            await Assert.That(tokeniser.GetCurrentIndex()).IsEqualTo(0);
            var testString1 = @"1 test";
            tokeniser.CallReset(testString1);
            await Assert.That(tokeniser.GetFullText()).IsEqualTo(testString1);
            await Assert.That(tokeniser.GetCurrentIndex()).IsEqualTo(0);
            await Assert.That(tokeniser.GetCurrentChar()).IsEqualTo(testString1[0]);
            var testString2 = @"2 test";
            tokeniser.CallReset(testString2);
            await Assert.That(tokeniser.GetFullText()).IsEqualTo(testString2);
            await Assert.That(tokeniser.GetCurrentIndex()).IsEqualTo(0);
            await Assert.That(tokeniser.GetCurrentChar()).IsEqualTo(testString2[0]);
        }

        [Test]
        public async Task Test_CallReadQuotedString_Reads_Strings()
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
                await Assert.That(result).IsEqualTo(testString.Value);
                await Assert.That(tokeniser.GetIsComplete()).IsTrue();
            }
        }

        [Test]
        public async Task Test_CallReadUnsignedInteger_Reads_Numbers()
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
                await Assert.That(result).IsEqualTo(testString.Value);
                await Assert.That(tokeniser.GetIsComplete()).IsTrue();
            }
        }

        [Test]
        public async Task Test_ReadValue_Reads_Quoted_Strings()
        {
            var tests = new Dictionary<string, object>
                {
                    { "\'Foo Bar\'", "Foo Bar" },
                    { "\"Foo Bar\"", "Foo Bar" },
                    { "\'Foo \\\' \\\" Bar\'", "Foo \' \" Bar" },
                    { "\"Foo \\\' \\\" Bar\"", "Foo \' \" Bar" },
                };
            await DoReadValueTests(tests);
        }

        private async Task DoReadValueTests(Dictionary<string, object> tests)
        {
            foreach (var t in tests)
                await DoReadValueTest(t.Key, t.Value);
        }

        private async Task DoReadValueTest(string input, object expectedOutput)
        {
            var parser = new Parser();
            parser.CallReset(input);
            var output = parser.CallReadValue();
            await Assert.That(output).IsEqualTo(expectedOutput);
            await Assert.That(parser.GetIsComplete()).IsTrue();
        }

        [Test]
        public async Task Test_ReadValue_Reads_Booleans()
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
            await DoReadValueTests(tests);
        }

        [Test]
        public async Task Test_ReadTextUntilWhitespaceOr_ReadsText()
        {
            var tests = new string[] { "fred;", "fred life;", "fred@test;", "fred", "fred\tlife;", "fred\n" };
            foreach (var test in tests)
            {
                var parser = new Parser();
                parser.CallReset(test);
                var output = parser.CallReadTextUntilWhitespaceOr('@', ';', ')');
                await Assert.That(output).IsEqualTo("fred");
            }
        }

        [Test]
        public async Task Test_ReadValue_Reads_Integers()
        {
            var tests = new[] { 0, 1, 2, 3, -123, -1, long.MinValue, long.MaxValue };
            var dict = tests.ToDictionary(x => x.ToString(CultureInfo.InvariantCulture),
                                          x => (object)x);
            await DoReadValueTests(dict);
        }

        [Test]
        public async Task Test_ReadValue_Reads_Doubles()
        {
            // note - we don't run any tests on things like double.MinValue - those would fail due to rounding errors.
            var tests = new[] { 0.001, 1.123, 2.2343, -123.1232, -1.2323, -99999.93454, 9999.343455 };
            var dict = tests.ToDictionary(x => x.ToString(CultureInfo.InvariantCulture),
                                          x => (object)x);
            await DoReadValueTests(dict);
        }

        public enum MyEnum
        {
            Value1,
            Value2,
            MyEnumValue3Foo
        }

        private async Task DoReadEnumerationTest(Type enumerationType, string input, object expectedOutput)
        {
            var parser = new Parser();
            parser.CallReset(input);
            var output = parser.CallReadEnumerationValue(enumerationType, true /* ignoreCase */);
            await Assert.That(output).IsEqualTo(expectedOutput);
            await Assert.That(parser.GetIsComplete()).IsTrue();
        }

        [Test]
        public async Task Test_ReadEnumeration_Reads_Enumerations()
        {
            foreach (var value in Enum.GetValues(typeof(MyEnum)))
            {
                await DoReadEnumerationTest(typeof(MyEnum), value.ToString(), value);
                await DoReadEnumerationTest(typeof(MyEnum), value.ToString().ToUpper(), value);
                await DoReadEnumerationTest(typeof(MyEnum), value.ToString().ToLower(), value);
            }
        }

        [Test]
        public async Task Test_ReadValidCSharpName_Succeeds()
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
                await Assert.That(result).IsEqualTo(name);
                await Assert.That(parser.GetIsComplete()).IsTrue();

                var parser2 = new Parser();
                parser2.CallReset("\t " + name + " \r \t");
                var result2 = parser2.CallReadValidCSharpName();
                await Assert.That(result2).IsEqualTo(name);
                await Assert.That(parser2.GetIsComplete()).IsFalse();
            }
        }

        [Test]
        public async Task Test_ReadValidCSharpName_Fails()
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
                await Assert.That(exceptionThrown).IsTrue();
            }
        }

        [Test]
        public async Task Reset_Resets_FullText_And_Index()
        {
            var parser = new Parser();
            parser.CallReset("derp");

            await Assert.That(parser.GetCurrentIndex()).IsEqualTo(0);
            await Assert.That(parser.GetFullText()).IsEqualTo("derp");
        }

        [Test]
        [Arguments('a')]
        [Arguments('b')]
        [Arguments('c')]
        [Arguments('d')]
        [Arguments('e')]
        [Arguments('f')]
        [Arguments('g')]
        [Arguments('_')]
        [Arguments('Z')]
        [Arguments('Æ')]
        [Arguments('ø')]
        [Arguments('ä')]
        [Arguments('µ')]
        public async Task TestIsValidFirstCharacterOfCSharpName_OK(char character)
        {
            var parser = new Parser();
            var result = parser.CallIsValidFirstCharacterOfCSharpName(character);
            await Assert.That(result).IsTrue();
        }

        [Test]
        [Arguments('1')]
        [Arguments(' ')]
        [Arguments('-')]
        [Arguments('\\')]
        [Arguments('^')]
        [Arguments('%')]
        [Arguments('$')]
        [Arguments('#')]
        [Arguments('@')]
        [Arguments('!')]
        [Arguments('&')]
        [Arguments('*')]
        [Arguments('(')]
        [Arguments(')')]
        [Arguments('+')]
        [Arguments('=')]
        public async Task TestIsValidFirstCharacterOfCSharpName_Fail(char character)
        {
            var parser = new Parser();
            var result = parser.CallIsValidFirstCharacterOfCSharpName(character);
            await Assert.That(result).IsFalse();
        }
    }
}
