﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Test;
using MvvmCross.UI;
using Xunit;

namespace MvvmCross.Plugins.Color.UnitTest
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
            public WrappedColor(MvxColor color)
            {
                Color = color;
            }

            public MvxColor Color { get; private set; }
        }

        public class MockNative : IMvxNativeColor
        {
            public object ToNative(MvxColor mvxColor)
            {
                return new WrappedColor(mvxColor);
            }
        }
    }
}
