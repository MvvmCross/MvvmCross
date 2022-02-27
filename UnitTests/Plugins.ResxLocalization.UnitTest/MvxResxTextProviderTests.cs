// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Plugin.ResxLocalization;
using MvvmCross.Plugin.ResxLocalization.UnitTest.Mocks;
using Xunit;

namespace MvvmCross.Plugin.ResxLocalization.UnitTest
{

    public class MvxResxTextProviderTests
    {
        private MockResourceManager _resourceManager;


        public MvxResxTextProviderTests()
        {
            _resourceManager = new MockResourceManager();
        }

        [Fact]
        public void GetTextForExistingValueSupplyingNameOnlyReturnsDummyName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = MockResourceManager.DummyName;

            var actual = textProvider.GetText(null, null, MockResourceManager.DummyName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTextSupplyingNamespaceAndNameReturnsValueMatchingNamespaceAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.DummyName}";

            var actual = textProvider.GetText(MockResourceManager.LocalizationNamespace, null, MockResourceManager.DummyName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTextSupplyingTypeKeyAndNameReturnsValueMatchingTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            var actual = textProvider.GetText(null, MockResourceManager.TypeKey, MockResourceManager.DummyName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTextSupplyingNamespaceAndTypeKeyAndNameReturnsValueMatchingNamespaceAndTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            var actual = textProvider.GetText(MockResourceManager.LocalizationNamespace, MockResourceManager.TypeKey, MockResourceManager.DummyName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTextForNonExistingValueReturnsNull()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);

            var actual = textProvider.GetText(null, null, "NonExistingKey");
            Assert.Null(actual);
        }

        [Fact]
        public void TryGetTextForExistingValueSupplyingNameOnlyReturnsTrue()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = MockResourceManager.DummyName;

            string actual;
            Assert.True(textProvider.TryGetText(out actual, null, null, MockResourceManager.DummyName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryGetTextSupplyingNamespaceAndNameOutputsValueMatchingNamespaceAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.DummyName}";

            string actual;
            Assert.True(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, null, MockResourceManager.DummyName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryGetTextSupplyingTypeKeyAndNameOutputsValueMatchingTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            string actual;
            Assert.True(textProvider.TryGetText(out actual, null, MockResourceManager.TypeKey, MockResourceManager.DummyName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryGetTextSupplyingNamespaceAndTypeKeyAndNameOutputsValueMatchingNamespaceAndTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            string actual;
            Assert.True(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, MockResourceManager.TypeKey, MockResourceManager.DummyName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryGetTextForNonExistingValueReturnsFalse()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);

            string actual;
            Assert.False(textProvider.TryGetText(out actual, null, null, "NonExistingKey"));
            Assert.Null(actual);
        }
    }
}
