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

        private const string StateUnknown = "State Unknown";
        private const string StateRunning = "State Running";
        private const string StateCompleted = "State Completed";
        private const string Fallback = "State fallback";

        public enum TestStates
        {
            Unknown,
            Running,
            Completed,
            Failed
        }

        [SetUp]
        public void Init()
        {
            _testStatedictionary = new Dictionary<TestStates, string>
            {
                [TestStates.Unknown] = StateUnknown,
                [TestStates.Running] = StateRunning,
                [TestStates.Completed] = StateCompleted
            };
        }

        [TestCase(TestStates.Unknown, ExpectedResult = StateUnknown)]
        [TestCase(TestStates.Running, ExpectedResult = StateRunning)]
        [TestCase(TestStates.Completed, ExpectedResult = StateCompleted)]
        public string Convert_MatchingKey_ReturnsDictionaryValue(TestStates state)
        {
            return Convert(state, null, new Tuple<IDictionary<TestStates, string>, string>(_testStatedictionary, Fallback), CultureInfo.CurrentUICulture);
        }

        [TestCase(TestStates.Failed, ExpectedResult = Fallback)]
        public string Convert_NoMatchingDictionaryKey_ReturnsFallback(TestStates state)
        {
            return Convert(state, null, new Tuple<IDictionary<TestStates, string>, string>(_testStatedictionary, Fallback), CultureInfo.CurrentUICulture);
        }

        [Test]
        public void Convert_InvalidParamterType_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Convert(TestStates.Unknown, null, _testStatedictionary, CultureInfo.CurrentUICulture));
        }
    }
}
