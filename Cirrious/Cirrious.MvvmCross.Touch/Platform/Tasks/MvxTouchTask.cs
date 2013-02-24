// MvxTouchTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.ServiceProvider;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Platform.Tasks
{
    public class MvxTouchTask
        : IMvxConsumer
    {
        protected bool DoUrlOpen(NSUrl url)
        {
            return UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}