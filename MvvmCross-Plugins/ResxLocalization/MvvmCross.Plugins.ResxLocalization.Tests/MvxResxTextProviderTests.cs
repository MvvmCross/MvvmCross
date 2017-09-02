using System.Collections.Generic;
using MvvmCross.Plugins.ResxLocalization.Tests.Mocks;
using NUnit.Framework;

namespace MvvmCross.Plugins.ResxLocalization.Tests
{
    [TestFixture]
    public class MvxResxTextProviderTests
    {
        private MockResourceManager _resourceManager;

        [SetUp]
        public void SetUp()
        {
            _resourceManager = new MockResourceManager();
        }

        [TearDown]
        public void TearDown()
        {
        }

        #region Tests covering the 'TryGetText' method

        [Test]
        public void TryGetTextForExistingValueSupplyingNameOnlyReturnsTrue()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = MockResourceManager.DummyName;

            string actual;
            Assert.IsTrue(textProvider.TryGetText(out actual, null, null, MockResourceManager.DummyName));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryGetTextSupplyingNamespaceAndNameOutputsValueMatchingNamespaceAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.DummyName}";

            string actual;
            Assert.IsTrue(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, null, MockResourceManager.DummyName));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryGetTextSupplyingTypeKeyAndNameOutputsValueMatchingTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            string actual;
            Assert.IsTrue(textProvider.TryGetText(out actual, null, MockResourceManager.TypeKey, MockResourceManager.DummyName));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryGetTextSupplyingNamespaceAndTypeKeyAndNameOutputsValueMatchingNamespaceAndTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            string actual;
            Assert.IsTrue(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, MockResourceManager.TypeKey, MockResourceManager.DummyName));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryGetTextSupplyingNamespaceOnlyReturnsFalse()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            
            string actual;
            Assert.IsFalse(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, null, null));
            Assert.IsNull(actual);
        }

        [Test]
        public void TryGetTextSupplyingTypeKeyOnlyReturnsFalse()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);

            string actual;
            Assert.IsFalse(textProvider.TryGetText(out actual, null, MockResourceManager.TypeKey, null));
            Assert.IsNull(actual);
        }

        [Test]
        public void TryGetTextSupplyingNamespaceAndTypeKeyReturnsFalse()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);

            string actual;
            Assert.IsFalse(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, MockResourceManager.TypeKey, null));
            Assert.IsNull(actual);
        }

        [Test]
        public void TryGetTextForNonExistingValueReturnsFalse()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);

            string actual;
            Assert.IsFalse(textProvider.TryGetText(out actual, null, null, "NonExistingKey"));
            Assert.IsNull(actual);
        }

        #endregion
    }
}
