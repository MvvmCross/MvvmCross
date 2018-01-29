// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.UI;
using MvvmCross.Test.Core;

namespace MvvmCross.Plugins.Color.Test
{
    public class MvxColorValueConverterTest : MvxIoCSupportingTest
    {
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

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();
            Ioc.RegisterSingleton<IMvxNativeColor>(new MockNative());
        }
    }
}