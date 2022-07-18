// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Plugin.JsonLocalization.UnitTest.TestClasses;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.Plugin.JsonLocalization.UnitTest
{
    public class MvxDictionaryTextProviderTest : IClassFixture<MvxTestFixture>
    {
        private readonly MvxTestFixture _fixture;

        public MvxDictionaryTextProviderTest(MvxTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetTextWithExistingValueReturnsTheValueWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = "DummyValue";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "DummyKey");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTextWithNonExistingValueReturnsTheKeyWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "NonExistingKey");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTextWithExistingValueReturnsTheValueWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);
            var expected = "DummyValue";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "DummyKey");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTextWithNonExistingValueThrowsKeyNotFoundExceptionWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            Assert.Throws<KeyNotFoundException>(() => textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "NonExistingKey"));
        }

        [Fact]
        public void TryGetTextForExistingValueReturnsTrueWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            Assert.True(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "DummyKey"));
        }

        [Fact]
        public void TryGetTextForExistingValueReturnsTrueWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            Assert.True(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "DummyKey"));
        }

        [Fact]
        public void TryGetTextForNonExistingValueReturnsFalseWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            Assert.False(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "NonExistingKey"));
        }

        [Fact]
        public void TryGetTextForNonExistingValueReturnsFalseWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            Assert.False(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "NonExistingKey"));
        }

        [Fact]
        public void TryGetTextForNonExistingValueShouldOutputKeyWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            string actual;
            textProvider.TryGetText(out actual,
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey, "NonExistingKey");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryGetTextForNonExistingValueShouldOutputKeyWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            string actual;
            textProvider.TryGetText(out actual,
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey, "NonExistingKey");

            Assert.Equal(expected, actual);
        }
    }
}
