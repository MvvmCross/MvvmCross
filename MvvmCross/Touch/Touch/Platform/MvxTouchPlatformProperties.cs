// MvxTouchPlatformProperties.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using ObjCRuntime;
using System;
using UIKit;

namespace Cirrious.MvvmCross.Touch.Platform
{
    [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
    public class MvxTouchPlatformProperties : IMvxTouchPlatformProperties
    {
        #region Implementation of IMvxTouchPlatformProperties

        public MvxTouchFormFactor FormFactor
        {
            get
            {
                switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
                {
                    case UIUserInterfaceIdiom.Phone:
                        if (UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale >= 1136)
                            return MvxTouchFormFactor.TallPhone;

                        return MvxTouchFormFactor.Phone;

                    case UIUserInterfaceIdiom.Pad:
                        return MvxTouchFormFactor.Pad;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public MvxTouchDisplayDensity DisplayDensity
        {
            get
            {
                if (UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
                {
                    var scale = (int)Math.Round(UIScreen.MainScreen.Scale);
                    if (scale == 2)
                    {
                        return MvxTouchDisplayDensity.Retina;
                    }
                }

                return MvxTouchDisplayDensity.Normal;
            }
        }

        #endregion Implementation of IMvxTouchPlatformProperties
    }
}