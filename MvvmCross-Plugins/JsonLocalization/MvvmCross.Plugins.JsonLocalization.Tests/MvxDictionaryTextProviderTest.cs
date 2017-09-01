using MvvmCross.Plugins.JsonLocalization.Tests.Mocks;
using NUnit.Framework;

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

        #region Tests covering the 'TryGetText' method

        [Test]
        public void TryGetExistingValueReturnsTrueWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            Assert.IsTrue(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "DummyKey"));
        }

        [Test]
        public void TryGetExistingValueReturnsTrueWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            Assert.IsTrue(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "DummyKey"));
        }

        [Test]
        public void TryGetNonExistingValueReturnsFalseWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            Assert.IsFalse(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "NonExistingKey"));
        }

        [Test]
        public void TryGetNonExistingValueReturnsFalseWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            Assert.IsFalse(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "NonExistingKey"));
        }

        [Test]
        public void TryGetNonExistingValueShouldOutputKeyWhenMaskingErrors()
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
        public void TryGetNonExistingValueShouldOutputKeyWhenNotMaskingErrors()
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
