#region Copyright
// <copyright file="MvxTouchPlatformProperties.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Touch.Interfaces;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Platform
{
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
                if (MonoTouch.UIKit.UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
                {
                    var scale = (int) Math.Round(MonoTouch.UIKit.UIScreen.MainScreen.Scale);
                    if (scale == 2)
                    {
                        return MvxTouchDisplayDensity.Retina;
                    }
                }

                return MvxTouchDisplayDensity.Normal;
            }
        }

        #endregion
    }
}