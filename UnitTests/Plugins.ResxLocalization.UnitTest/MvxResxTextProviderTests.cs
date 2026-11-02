// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Plugin.ResxLocalization;
using MvvmCross.Plugin.ResxLocalization.UnitTest.Mocks;

namespace MvvmCross.Plugin.ResxLocalization.UnitTest
{

    public class MvxResxTextProviderTests
    {
        private MockResourceManager _resourceManager;


        public MvxResxTextProviderTests()
        {
            _resourceManager = new MockResourceManager();
        }

        [Test]
        public async Task GetTextForExistingValueSupplyingNameOnlyReturnsDummyName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = MockResourceManager.DummyName;

            var actual = textProvider.GetText(null, null, MockResourceManager.DummyName);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task GetTextSupplyingNamespaceAndNameReturnsValueMatchingNamespaceAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.DummyName}";

            var actual = textProvider.GetText(MockResourceManager.LocalizationNamespace, null, MockResourceManager.DummyName);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task GetTextSupplyingTypeKeyAndNameReturnsValueMatchingTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            var actual = textProvider.GetText(null, MockResourceManager.TypeKey, MockResourceManager.DummyName);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task GetTextSupplyingNamespaceAndTypeKeyAndNameReturnsValueMatchingNamespaceAndTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            var actual = textProvider.GetText(MockResourceManager.LocalizationNamespace, MockResourceManager.TypeKey, MockResourceManager.DummyName);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task GetTextForNonExistingValueReturnsNull()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);

            var actual = textProvider.GetText(null, null, "NonExistingKey");
            await Assert.That(actual).IsNull();
        }

        [Test]
        public async Task TryGetTextForExistingValueSupplyingNameOnlyReturnsTrue()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = MockResourceManager.DummyName;

            string actual;
            await Assert.That(textProvider.TryGetText(out actual, null, null, MockResourceManager.DummyName)).IsTrue();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task TryGetTextSupplyingNamespaceAndNameOutputsValueMatchingNamespaceAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.DummyName}";

            string actual;
            await Assert.That(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, null, MockResourceManager.DummyName)).IsTrue();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task TryGetTextSupplyingTypeKeyAndNameOutputsValueMatchingTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            string actual;
            await Assert.That(textProvider.TryGetText(out actual, null, MockResourceManager.TypeKey, MockResourceManager.DummyName)).IsTrue();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task TryGetTextSupplyingNamespaceAndTypeKeyAndNameOutputsValueMatchingNamespaceAndTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            string actual;
            await Assert.That(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, MockResourceManager.TypeKey, MockResourceManager.DummyName)).IsTrue();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task TryGetTextForNonExistingValueReturnsFalse()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);

            string actual;
            await Assert.That(textProvider.TryGetText(out actual, null, null, "NonExistingKey")).IsFalse();
            await Assert.That(actual).IsNull();
        }
    }
}
