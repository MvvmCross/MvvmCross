// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.Forms.Views.Base;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Windows.ApplicationModel.Activation;
using Xamarin.Forms;

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

                var startup = Mvx.IoCProvider.Resolve<IMvxAppStart>();
                if (!startup.IsStarted)
                    startup.Start(GetAppStartHint(activationArgs));

                var hostType = HostWindowsPageType();
                RootFrame.Navigate(hostType, (activationArgs as LaunchActivatedEventArgs)?.Arguments);
            }
            else
                base.RunAppStart(activationArgs);
        }
    }

    public abstract class MvxWindowsApplication<TMvxUapSetup, TApplication, TFormsApplication> : MvxWindowsApplication
       where TMvxUapSetup : MvxFormsWindowsSetup<TApplication, TFormsApplication>, new()
       where TApplication : class, IMvxApplication, new()
        where TFormsApplication : Application, new()
    {
        protected abstract override Type HostWindowsPageType();

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxUapSetup>();
        }
    }

    public class MvxWindowsApplication<TMvxUapSetup, TApplication, TFormsApplication, THostPageType> : MvxWindowsApplication<TMvxUapSetup, TApplication, TFormsApplication>
       where TMvxUapSetup : MvxFormsWindowsSetup<TApplication, TFormsApplication>, new()
       where TApplication : class, IMvxApplication, new()
        where TFormsApplication : Application, new()
        where THostPageType : MvxFormsWindowsPage
    {
        protected override Type HostWindowsPageType()
        {
            return typeof(THostPageType);
        }
    }
}
