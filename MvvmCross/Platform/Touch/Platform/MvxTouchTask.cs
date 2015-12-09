// MvxTouchTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using UIKit;

namespace Cirrious.CrossCore.Touch.Platform
{
    public class MvxTouchTask
    {
        protected bool DoUrlOpen(NSUrl url)
        {
            return UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}