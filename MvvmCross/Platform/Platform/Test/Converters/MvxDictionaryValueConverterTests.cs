using MvvmCross.Platform.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MvvmCross.Platform.Test.Converters
{
    [TestFixture]
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

        [SetUp]
        public void Init()
        {
            _testStatedictionary = new Dictionary<TestStates, string>
            {
                [TestStates.Running] = StateRunning,
                [TestStates.Completed] = StateCompleted
            };
        }

        [TestCase(TestStates.Running, ExpectedResult = StateRunning)]
        [TestCase(TestStates.Completed, ExpectedResult = StateCompleted)]
        public string Convert_MatchingKeyIncludeFallback_ReturnsDictionaryValue(TestStates state)
        {
            return Convert(state, null, new Tuple<IDictionary<TestStates, string>, string, bool>(_testStatedictionary, Fallback, true), CultureInfo.CurrentUICulture);
        }

        [TestCase(TestStates.Running, ExpectedResult = StateRunning)]
        [TestCase(TestStates.Completed, ExpectedResult = StateCompleted)]
        public string Convert_MatchingKeyExcludeFallback_ReturnsDictionaryValue(TestStates state)
        {
            return Convert(state, null, new Tuple<IDictionary<TestStates, string>, string, bool>(_testStatedictionary, default(string), false), CultureInfo.CurrentUICulture);
        }

        [Test]
        public void Convert_NoMatchingDictionaryKeyIncludeFallback_ReturnsFallback()
        {
            var state = TestStates.Failed;

            var result = Convert(state, null, new Tuple<IDictionary<TestStates, string>, string, bool>(_testStatedictionary, Fallback, true), CultureInfo.CurrentUICulture);

            Assert.AreEqual(result, Fallback);
        }

        [Test]
        public void Convert_NoMatchingDictionaryKeyExcludeFallback_ThrowKeyNotFoundException()
        {
            var state = TestStates.Failed;

            Assert.Throws<KeyNotFoundException>(() =>
                Convert(state, null, new Tuple<IDictionary<TestStates, string>, string, bool>(_testStatedictionary, default(string), false), CultureInfo.CurrentUICulture));
        }

        [Test]
        public void Convert_InvalidParamterType_ThrowArgumentException()
        {
            var state = TestStates.Running;

            Assert.Throws<ArgumentException>(() => Convert(state, null, _testStatedictionary, CultureInfo.CurrentUICulture));
        }
    }
}
