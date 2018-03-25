﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Forms.Views.Base;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Windows.ApplicationModel.Activation;

namespace MvvmCross.Forms.Platforms.Uap.Views
{
    public abstract class MvxWindowsApplication : MvvmCross.Platforms.Uap.Views.MvxApplication
    {
        protected abstract Type HostWindowsPageType();

        protected override void RunAppStart(IActivatedEventArgs activationArgs)
        {
            if (RootFrame?.Content == null)
            {
                MvxWindowsSetupSingleton.EnsureSingletonAvailable(RootFrame, activationArgs, nameof(Suspend)).EnsureInitialized();
                
                var startup = Mvx.Resolve<IMvxAppStart>();
                if (!startup.IsStarted)
                    startup.Start(GetAppStartHint(activationArgs));

                var hostType = HostWindowsPageType();
                RootFrame.Navigate(hostType, (activationArgs as LaunchActivatedEventArgs)?.Arguments);
            }
            else
                base.RunAppStart(activationArgs);
        }
    }
}
