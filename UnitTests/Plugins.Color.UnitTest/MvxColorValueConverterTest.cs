// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Tests;
using MvvmCross.UI;
using Xunit;

namespace MvvmCross.Plugin.Color.UnitTest
{
    public class MvxColorValueConverterTest : IClassFixture<MvxTestFixture>
    {
        public MvxTestFixture Fixture { get; }

        public MvxColorValueConverterTest(MvxTestFixture fixture)
        {
            Fixture = fixture;
            Fixture.Ioc.RegisterSingleton<IMvxNativeColor>(new MockNative());
        }

        public class WrappedColor
        {
            public WrappedColor(System.Drawing.Color color)
            {
                Color = color;
            }

            public System.Drawing.Color Color { get; private set; }
        }

        public class MockNative : IMvxNativeColor
        {
            public object ToNative(System.Drawing.Color color)
            {
                return new WrappedColor(color);
            }
        }
    }
}
