// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Plugin.Visibility;
using MvvmCross.Tests;
using MvvmCross.UI;
using Xunit;

namespace MvvmCross.Plugins.Visibility.UnitTest
{
    [Collection("Visibility")]
    public class VisibilityValueConverterTest
    {
        MvxTestFixture _fixture;

        public VisibilityValueConverterTest(MvxTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();

            _fixture.Ioc.RegisterSingleton<IMvxNativeVisibility>(new MockNativeVisibility());
        }

        [Theory]
        [InlineData(true, false, MvxVisibility.Visible)]
        [InlineData(true, true, MvxVisibility.Visible)]
        [InlineData(false, false, MvxVisibility.Collapsed)]
        [InlineData(false, true, MvxVisibility.Hidden)]
        public void VisibilityValueConverter_Test(bool visible, bool hide, MvxVisibility expected)
        {
            var converter = new MvxVisibilityValueConverter();
            var result = converter.Convert(visible, typeof(bool), hide, CultureInfo.CurrentUICulture);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true, false, MvxVisibility.Collapsed)]
        [InlineData(true, true, MvxVisibility.Hidden)]
        [InlineData(false, false, MvxVisibility.Visible)]
        [InlineData(false, true, MvxVisibility.Visible)]
        public void InvertedVisibilityValueConverter_Test(bool visible, bool hide, MvxVisibility expected)
        {
            var converter = new MvxInvertedVisibilityValueConverter();
            var result = converter.Convert(visible, typeof(bool), hide, CultureInfo.CurrentUICulture);
            Assert.Equal(expected, result);
        }

        public class MockNativeVisibility : IMvxNativeVisibility
        {
            public object ToNative(MvxVisibility visibility) => visibility;
        }
    }
}
