// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using MvvmCross.Converters;
using Xunit;

namespace MvvmCross.UnitTest.Base.Converters
{
    public class MvxDictionaryValueConverterTests : MvxDictionaryValueConverter<MvxDictionaryValueConverterTests.TestStates, string>
    {
        private Dictionary<TestStates, string> _testStatedictionary;

        private const string StateRunning = "State Running";
        private const string StateCompleted = "State Completed";
        private const string Fallback = "State fallback";

        public enum TestStates
        {
            Running,
            Completed,
            Failed
        }

        public MvxDictionaryValueConverterTests()
        {
            _testStatedictionary = new Dictionary<TestStates, string>
            {
                [TestStates.Running] = StateRunning,
                [TestStates.Completed] = StateCompleted
            };
        }

        [Theory]
        [InlineData(TestStates.Running, StateRunning)]
        [InlineData(TestStates.Completed, StateCompleted)]
        public void Convert_MatchingKeyIncludeFallback_ReturnsDictionaryValue(TestStates state, string expected)
        {
            var converted = Convert(state, null, new Tuple<IDictionary<TestStates, string>, string, bool>(_testStatedictionary, Fallback, true), CultureInfo.CurrentUICulture);
            Assert.Equal(expected, converted);
        }

        [Theory]
        [InlineData(TestStates.Running, StateRunning)]
        [InlineData(TestStates.Completed, StateCompleted)]
        public void Convert_MatchingKeyExcludeFallback_ReturnsDictionaryValue(TestStates state, string expected)
        {
            var converted = Convert(state, null, new Tuple<IDictionary<TestStates, string>, string, bool>(_testStatedictionary, default(string), false), CultureInfo.CurrentUICulture);
            Assert.Equal(expected, converted);
        }

        [Fact]
        public void Convert_NoMatchingDictionaryKeyIncludeFallback_ReturnsFallback()
        {
            var state = TestStates.Failed;

            var result = Convert(state, null, new Tuple<IDictionary<TestStates, string>, string, bool>(_testStatedictionary, Fallback, true), CultureInfo.CurrentUICulture);

            Assert.Equal(result, Fallback);
        }

        [Fact]
        public void Convert_NoMatchingDictionaryKeyExcludeFallback_ThrowKeyNotFoundException()
        {
            var state = TestStates.Failed;

            Assert.Throws<KeyNotFoundException>(() =>
                Convert(state, null, new Tuple<IDictionary<TestStates, string>, string, bool>(_testStatedictionary, default(string), false), CultureInfo.CurrentUICulture));
        }

        [Fact]
        public void Convert_InvalidParamterType_ThrowArgumentException()
        {
            var state = TestStates.Running;

            Assert.Throws<ArgumentException>(() => Convert(state, null, _testStatedictionary, CultureInfo.CurrentUICulture));
        }
    }
}
