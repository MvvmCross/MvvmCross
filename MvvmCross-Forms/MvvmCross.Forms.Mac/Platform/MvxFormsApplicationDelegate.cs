// MvxFormsIosPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Martin Nygren, @zzcgumn, zzcgumn@me.com

using System;
using AppKit;
using Foundation;
using MvvmCross.Core.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.Mac.Platform;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace MvvmCross.Forms.Mac
{
    public abstract class MvxFormsApplicationDelegate : FormsApplicationDelegate, IMvxApplicationDelegate
    {
        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            Mvx.Resolve<IMvxFormsViewPresenter>().FormsApplication.SendStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            base.DidFinishLaunching(notification);
        }

        public override void WillBecomeActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidResignActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
            base.WillTerminate(notification);
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }
}