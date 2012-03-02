#region Copyright

// <copyright file="MvxTouchServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#region using

using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.Services.Location;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.Interfaces.Services.Tasks;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Touch.Platform.Location;
using Cirrious.MvvmCross.Touch.Platform.Tasks;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Interfaces.Services.Lifetime;

#endregion

namespace Cirrious.MvvmCross.Touch.Platform
{
#warning move this file into platform namespace	
	public class MvxApplicationDelegate : UIApplicationDelegate
		, IMvxLifetime
	{
		public override void WillEnterForeground (UIApplication application)
		{
			FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
		}
		
		public override void DidEnterBackground (UIApplication application)
		{
			FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
		}
		
		public override void WillTerminate (UIApplication application)
		{
			FireLifetimeChanged(MvxLifetimeEvent.Closing);
		}
		
		public override void FinishedLaunching (UIApplication application)
		{
			FireLifetimeChanged(MvxLifetimeEvent.Launching);
		}
		
		private void FireLifetimeChanged(MvxLifetimeEvent which)
		{
			var handler = LifetimeChanged;
			if (handler != null)
				handler(this, new MvxLifetimeEventArgs(which));
		}
		
		#region IMvxLifetime implementation
		
		public event System.EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
		
		#endregion
	}
	
    [MvxServiceProvider]
    public class MvxTouchServiceProvider : MvxPlatformIndependentServiceProvider
    {
        public override void Initialize(IMvxIoCProvider iocProvider)
        {
            base.Initialize(iocProvider);
            SetupPlatformTypes();
        }
		
		public static new MvxTouchServiceProvider Instance { get { return (MvxTouchServiceProvider)MvxPlatformIndependentServiceProvider.Instance;} }
		
        private void SetupPlatformTypes()
        {
			RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            RegisterServiceType<IMvxSimpleFileStoreService, MvxTouchFileStoreService>();
            RegisterServiceType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            RegisterServiceType<IMvxPhoneCallTask, MvxPhoneCallTask>();
            RegisterServiceType<IMvxResourceLoader, MvxTouchResourceLoader>();

#warning Would be very nice if GPS were optional!
            RegisterServiceInstance<IMvxGeoLocationWatcher>(new MvxTouchGeoLocationWatcher());

        }
		
		public void SetupAdditionalPlatformTypes(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
		{
			RegisterServiceInstance<IMvxLifetime>(applicationDelegate);
            RegisterServiceInstance<IMvxPictureChooserTask>(new MvxImagePickerTask(presenter));
        }
    }
}