using System.Collections.Generic;
using MvvmCross.Plugins.JsonLocalization.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MvvmCross.Plugins.JsonLocalization.Tests
{
    [TestFixture]
    public class MvxDictionaryTextProviderTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        #region Tests covertin the 'GetText' method

        [Test]
        public void GetTextWithExistingValueReturnsTheValueWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = "DummyValue";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey, 
                "DummyKey");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetTextWithNonExistingValueReturnsTheKeyWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "NonExistingKey");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetTextWithExistingValueReturnsTheValueWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);
            var expected = "DummyValue";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "DummyKey");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetTextWithNonExistingValueThrowsKeyNotFoundExceptionWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);
            
            Assert.Throws<KeyNotFoundException>(() => textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "NonExistingKey"));
        }

        #endregion

        #region Tests covering the 'TryGetText' method

        [Test]
        public void TryGetTextForExistingValueReturnsTrueWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            Assert.IsTrue(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "DummyKey"));
        }

        [Test]
        public void TryGetTextForExistingValueReturnsTrueWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            Assert.IsTrue(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "DummyKey"));
        }

        [Test]
        public void TryGetTextForNonExistingValueReturnsFalseWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            Assert.IsFalse(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "NonExistingKey"));
        }

        [Test]
        public void TryGetTextForNonExistingValueReturnsFalseWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            Assert.IsFalse(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "NonExistingKey"));
        }

        [Test]
        public void TryGetTextForNonExistingValueShouldOutputKeyWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            string actual;
            textProvider.TryGetText(out actual, 
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey, "NonExistingKey");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryGetTextForNonExistingValueShouldOutputKeyWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            string actual;
            textProvider.TryGetText(out actual,
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey, "NonExistingKey");

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
