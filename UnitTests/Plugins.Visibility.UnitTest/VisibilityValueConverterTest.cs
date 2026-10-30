// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Plugin.Visibility;
using MvvmCross.Tests;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility.UnitTest
{
    [ClassDataSource<MvxTestFixture>(Shared = SharedType.Keyed, Key = "Visibility")]
    public class VisibilityValueConverterTest
    {
        MvxTestFixture _fixture;

        public VisibilityValueConverterTest(MvxTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();

            _fixture.Ioc.RegisterSingleton<IMvxNativeVisibility>(new MockNativeVisibility());
        }

        [Test]
        [Arguments(true, false, MvxVisibility.Visible)]
        [Arguments(true, true, MvxVisibility.Visible)]
        [Arguments(false, false, MvxVisibility.Collapsed)]
        [Arguments(false, true, MvxVisibility.Hidden)]
        public async Task VisibilityValueConverter_Test(bool visible, bool hide, MvxVisibility expected)
        {
            var converter = new MvxVisibilityValueConverter();
            var result = converter.Convert(visible, typeof(bool), hide, CultureInfo.CurrentUICulture);
            await Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        [Arguments(true, false, MvxVisibility.Collapsed)]
        [Arguments(true, true, MvxVisibility.Hidden)]
        [Arguments(false, false, MvxVisibility.Visible)]
        [Arguments(false, true, MvxVisibility.Visible)]
        public async Task InvertedVisibilityValueConverter_Test(bool visible, bool hide, MvxVisibility expected)
        {
            var converter = new MvxInvertedVisibilityValueConverter();
            var result = converter.Convert(visible, typeof(bool), hide, CultureInfo.CurrentUICulture);
            await Assert.That(result).IsEqualTo(expected);
        }

        public class MockNativeVisibility : IMvxNativeVisibility
        {
            public object ToNative(MvxVisibility visibility) => visibility;
        }
    }
}
