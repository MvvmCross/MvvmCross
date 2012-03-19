#region Copyright
// <copyright file="MvxWindowsPhoneServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Interfaces.Platform.Location;
using Cirrious.MvvmCross.Interfaces.Platform.SoundEffects;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.WindowsPhone.Platform.Bookmarks;
using Cirrious.MvvmCross.WindowsPhone.Platform.Lifetime;
using Cirrious.MvvmCross.WindowsPhone.Platform.Location;
using Cirrious.MvvmCross.WindowsPhone.Platform.SoundEffects;
using Cirrious.MvvmCross.WindowsPhone.Platform.Tasks;

#endregion

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    [MvxServiceProvider]
    public class MvxWindowsPhoneServiceProvider : MvxPlatformIndependentServiceProvider
    {
        public MvxWindowsPhoneServiceProvider()
        {
        }

        public override void Initialize(IMvxIoCProvider iocProvider)
        {
            base.Initialize(iocProvider);
            SetupPlatformTypes();
        }

        private void SetupPlatformTypes()
        {
            RegisterServiceInstance<IMvxLifetime>(new MvxWindowsPhoneLifetimeMonitor());
            RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            RegisterServiceType<IMvxSimpleFileStoreService, MvxIsolatedStorageFileStoreService>();
            RegisterServiceType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            RegisterServiceType<IMvxPhoneCallTask, MvxPhoneCallTask>();
            RegisterServiceType<IMvxComposeEmailTask, MvxComposeEmailTask>();
            RegisterServiceType<IMvxShareTask, MvxShareTask>();
            RegisterServiceType<IMvxPictureChooserTask, MvxPictureChooserTask>();
            RegisterServiceType<IMvxCombinedPictureChooserTask, MvxPictureChooserTask>();            
            RegisterServiceType<IMvxResourceLoader, MvxWindowsPhoneResourceLoader>();
            RegisterServiceType<IMvxBookmarkLibrarian, MvxWindowsPhoneLiveTileBookmarkLibrarian>();

#warning Would be nice if sound effects were optional so that not everyone has to link to xna!
            var soundEffectLoader = new MvxSoundEffectObjectLoader();
            RegisterServiceInstance<IMvxResourceObjectLoaderConfiguration<IMvxSoundEffect>>(soundEffectLoader);
            RegisterServiceInstance<IMvxResourceObjectLoader<IMvxSoundEffect>>(soundEffectLoader);

#warning Would be very nice if GPS were optional!
            RegisterServiceInstance<IMvxGeoLocationWatcher>(new MvxWindowsPhoneGeoLocationWatcher());
        }
    }
}