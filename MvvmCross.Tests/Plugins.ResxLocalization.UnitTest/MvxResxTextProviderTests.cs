// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

﻿using MvvmCross.Plugins.ResxLocalization.Tests.Mocks;
using Xunit;

namespace MvvmCross.Plugins.ResxLocalization.Tests
{
    
    public class MvxResxTextProviderTests
    {
        private MockResourceManager _resourceManager;

        
        public void SetUp()
        {
            _resourceManager = new MockResourceManager();
        }

        [TearDown]
        public void TearDown()
        {
        }

        #region Tests covering the 'GetText' method

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
            Assert.IsNull(actual);
        }

        #endregion

        #region Tests covering the 'TryGetText' method

        [Fact]
        public void TryGetTextForExistingValueSupplyingNameOnlyReturnsTrue()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = MockResourceManager.DummyName;

            string actual;
            Assert.IsTrue(textProvider.TryGetText(out actual, null, null, MockResourceManager.DummyName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryGetTextSupplyingNamespaceAndNameOutputsValueMatchingNamespaceAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.DummyName}";

            string actual;
            Assert.IsTrue(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, null, MockResourceManager.DummyName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryGetTextSupplyingTypeKeyAndNameOutputsValueMatchingTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            string actual;
            Assert.IsTrue(textProvider.TryGetText(out actual, null, MockResourceManager.TypeKey, MockResourceManager.DummyName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryGetTextSupplyingNamespaceAndTypeKeyAndNameOutputsValueMatchingNamespaceAndTypeKeyAndName()
        {
            var textProvider = new MvxResxTextProvider(_resourceManager);
            var expected = $"{MockResourceManager.LocalizationNamespace}.{MockResourceManager.TypeKey}.{MockResourceManager.DummyName}";

            string actual;
            Assert.IsTrue(textProvider.TryGetText(out actual, MockResourceManager.LocalizationNamespace, MockResourceManager.TypeKey, MockResourceManager.DummyName));
            Assert.Equal(expected, actual);
        }

        [Fact]
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
