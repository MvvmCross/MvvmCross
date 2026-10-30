// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Plugin.JsonLocalization.UnitTest.TestClasses;
using MvvmCross.Tests;

namespace MvvmCross.Plugin.JsonLocalization.UnitTest
{
    [ClassDataSource<MvxTestFixture>(Shared = SharedType.PerClass)]
    public class MvxDictionaryTextProviderTest
    {
        private readonly MvxTestFixture _fixture;

        public MvxDictionaryTextProviderTest(MvxTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Test]
        public async Task GetTextWithExistingValueReturnsTheValueWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = "DummyValue";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "DummyKey");

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task GetTextWithNonExistingValueReturnsTheKeyWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "NonExistingKey");

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task GetTextWithExistingValueReturnsTheValueWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);
            var expected = "DummyValue";

            var actual = textProvider.GetText(
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey,
                "DummyKey");

            await Assert.That(actual).IsEqualTo(expected);
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

        [Test]
        public async Task TryGetTextForExistingValueReturnsTrueWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            await Assert.That(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "DummyKey")).IsTrue();
        }

        [Test]
        public async Task TryGetTextForExistingValueReturnsTrueWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            await Assert.That(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "DummyKey")).IsTrue();
        }

        [Test]
        public async Task TryGetTextForNonExistingValueReturnsFalseWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);

            string value;
            await Assert.That(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "NonExistingKey")).IsFalse();
        }

        [Test]
        public async Task TryGetTextForNonExistingValueReturnsFalseWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);

            string value;
            await Assert.That(textProvider.TryGetText(out value, TestDictionaryTextProvider.LocalizationNamespace, TestDictionaryTextProvider.TypeKey, "NonExistingKey")).IsFalse();
        }

        [Test]
        public async Task TryGetTextForNonExistingValueShouldOutputKeyWhenMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(true);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            string actual;
            textProvider.TryGetText(out actual,
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey, "NonExistingKey");

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task TryGetTextForNonExistingValueShouldOutputKeyWhenNotMaskingErrors()
        {
            var textProvider = TestDictionaryTextProvider.CreateAndInitializeWithDummyData(false);
            var expected = $"{TestDictionaryTextProvider.LocalizationNamespace}|{TestDictionaryTextProvider.TypeKey}|NonExistingKey";

            string actual;
            textProvider.TryGetText(out actual,
                TestDictionaryTextProvider.LocalizationNamespace,
                TestDictionaryTextProvider.TypeKey, "NonExistingKey");

            await Assert.That(actual).IsEqualTo(expected);
        }
    }
}
