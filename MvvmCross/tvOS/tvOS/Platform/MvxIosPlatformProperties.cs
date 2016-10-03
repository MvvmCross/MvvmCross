// MvxIosPlatformProperties.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Platform
{
    using System;

    using ObjCRuntime;

    using UIKit;

    [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
    public class MvxIosPlatformProperties : IMvxIosPlatformProperties
    {
        #region Implementation of IMvxIosPlatformProperties

        public MvxIosFormFactor FormFactor
        {
            get
            {
                switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
                {
                    case UIUserInterfaceIdiom.Phone:
                        if (UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale >= 1136)
                            return MvxIosFormFactor.TallPhone;

                        return MvxIosFormFactor.Phone;

                    case UIUserInterfaceIdiom.Pad:
                        return MvxIosFormFactor.Pad;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public MvxIosDisplayDensity DisplayDensity
        {
            get
            {
                if (UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
                {
                    var scale = (int)Math.Round(UIScreen.MainScreen.Scale);
                    if (scale == 2)
                    {
                        return MvxIosDisplayDensity.Retina;
                    }
                }

                return MvxIosDisplayDensity.Normal;
            }
        }

        #endregion Implementation of IMvxIosPlatformProperties
    }
}