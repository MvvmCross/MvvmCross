// MvxTvosPlatformProperties.cs

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
    public class MvxTvosPlatformProperties : IMvxTvosPlatformProperties
    {
        #region Implementation of IMvxTvosPlatformProperties

        public MvxTvosFormFactor FormFactor
        {
            get
            {
                switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
                {
                    case UIUserInterfaceIdiom.Phone:
                        if (UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale >= 1136)
                            return MvxTvosFormFactor.TallPhone;

                        return MvxTvosFormFactor.Phone;

                    case UIUserInterfaceIdiom.Pad:
                        return MvxTvosFormFactor.Pad;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public MvxTvosDisplayDensity DisplayDensity
        {
            get
            {
                if (UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
                {
                    var scale = (int)Math.Round(UIScreen.MainScreen.Scale);
                    if (scale == 2)
                    {
                        return MvxTvosDisplayDensity.Retina;
                    }
                }

                return MvxTvosDisplayDensity.Normal;
            }
        }

        #endregion Implementation of IMvxTvosPlatformProperties
    }
}