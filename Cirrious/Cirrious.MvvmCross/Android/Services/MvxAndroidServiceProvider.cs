#region Copyright

// <copyright file="MvxAndroidServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#region using

using Android.Content;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Android.LifeTime;
using Cirrious.MvvmCross.Android.Services.Location;
using Cirrious.MvvmCross.Android.Services.Tasks;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.Interfaces.Services.Lifetime;
using Cirrious.MvvmCross.Interfaces.Services.Location;
using Cirrious.MvvmCross.Interfaces.Services.Tasks;
using Cirrious.MvvmCross.Platform;

#endregion

namespace Cirrious.MvvmCross.Android.Services
{
    [MvxServiceProvider]
    public class MvxAndroidServiceProvider : MvxPlatformIndependentServiceProvider
    {
        public override void Initialize(IMvxIoCProvider iocProvider)
        {
            base.Initialize(iocProvider);
            RegisterPlatformTypes();
        }

        public static new MvxAndroidServiceProvider Instance
        {
            get { return (MvxAndroidServiceProvider)MvxPlatformIndependentServiceProvider.Instance; }
        }

        private void RegisterPlatformTypes()
        {
            RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            RegisterServiceType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            RegisterServiceType<IMvxPhoneCallTask, MvxPhoneCallTask>();
            
            var lifetimeMonitor = new MvxAndroidLifetimeMonitor();
            RegisterServiceInstance<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            RegisterServiceInstance<IMvxAndroidCurrentTopActivity>(lifetimeMonitor);
            RegisterServiceInstance<IMvxLifetime>(lifetimeMonitor);
        }

        public void RegisterPlatformContextTypes(Context applicationContext)
        {
            RegisterServiceInstance<IMvxResourceLoader>(new MvxAndroidResourceLoader());
            RegisterServiceInstance<IMvxSimpleFileStoreService>(new MvxAndroidFileStoreService());

            RegisterServiceType<IMvxPictureChooserTask, MvxPictureChooserTask>();

#warning Would be nice if sound effects were optional so that not everyone has to link to xna!
#warning TODO - sound effects!
            //var soundEffectLoader = new SoundEffects.MvxSoundEffectObjectLoader();
            //RegisterServiceInstance<IMvxResourceObjectLoaderConfiguration<IMvxSoundEffect>>(soundEffectLoader);
            //RegisterServiceInstance<IMvxResourceObjectLoader<IMvxSoundEffect>>(soundEffectLoader);

#warning Would be very nice if GPS were optional!
            RegisterServiceInstance<IMvxGeoLocationWatcher>(new MvxAndroidGeoLocationWatcher());
        }
    }
}