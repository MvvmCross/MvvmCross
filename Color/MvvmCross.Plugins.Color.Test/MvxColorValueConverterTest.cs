// MvxColorValueConverterTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;
using Cirrious.MvvmCross.Test.Core;

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