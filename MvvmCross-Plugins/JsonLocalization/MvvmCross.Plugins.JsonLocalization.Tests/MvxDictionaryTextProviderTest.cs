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

        [Test]
        public void TryGetExistingValueReturnsTrueWhenMaskingErrors()
        {
            var textProvider = MockMvxDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            Assert.IsTrue(textProvider.TryGetText(out value, MockMvxDictionaryTextProvider.LocalizationNamespace, MockMvxDictionaryTextProvider.TypeKey, "DummyKey"));
        }

        [Test]
        public void TryGetExistingValueReturnsTrueWhenNotMaskingErrors()
        {
            var textProvider = MockMvxDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            Assert.IsTrue(textProvider.TryGetText(out value, MockMvxDictionaryTextProvider.LocalizationNamespace, MockMvxDictionaryTextProvider.TypeKey, "DummyKey"));
        }

        [Test]
        public void TryGetNonExistingValueReturnsFalseWhenMaskingErrors()
        {
            var textProvider = MockMvxDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            Assert.IsFalse(textProvider.TryGetText(out value, MockMvxDictionaryTextProvider.LocalizationNamespace, MockMvxDictionaryTextProvider.TypeKey, "NonExistingKey"));
        }

        [Test]
        public void TryGetNonExistingValueReturnsFalseWhenNotMaskingErrors()
        {
            var textProvider = MockMvxDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            Assert.IsFalse(textProvider.TryGetText(out value, MockMvxDictionaryTextProvider.LocalizationNamespace, MockMvxDictionaryTextProvider.TypeKey, "NonExistingKey"));
        }
    }
}
