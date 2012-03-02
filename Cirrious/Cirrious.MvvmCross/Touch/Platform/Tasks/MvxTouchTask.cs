#region Copyright
// <copyright file="MvxTouchTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Platform.Tasks
{
    public class MvxTouchTask
        : IMvxServiceConsumer<IMvxViewDispatcherProvider>
    {
        protected bool DoUrlOpen(NSUrl url)
		{
#warning What exceptions could be thrown here?
#warning Does this need to be on UI thread?
			return UIApplication.SharedApplication.OpenUrl(url);
		}
    }
}