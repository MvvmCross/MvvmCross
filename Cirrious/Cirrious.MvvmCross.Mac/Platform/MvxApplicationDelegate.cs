#region Copyright
// <copyright file="MvxApplicationDelegate.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Mac.Platform
{
    public class MvxApplicationDelegate : NSApplicationDelegate
                                          , IMvxLifetime
    {
#warning Removed this lifetime functionality....
		/*
        public override void WillEnterForeground (NSApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }
		
        public override void DidEnterBackground (NSApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }
		
        public override void WillTerminate (NSApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
        }
		
        public override void FinishedLaunching (NSApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
        }
		*/
        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            if (handler != null)
                handler(this, new MvxLifetimeEventArgs(which));
        }

        #region IMvxLifetime implementation
		
        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
		
        #endregion
    }
}