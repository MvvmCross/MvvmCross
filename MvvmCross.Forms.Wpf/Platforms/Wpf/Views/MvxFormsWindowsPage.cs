// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Windows;
using MvvmCross.Forms.Platforms.Wpf.Presenters;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Xamarin.Forms.Platform.WPF;

namespace MvvmCross.Forms.Platforms.Wpf.Views
{
    public class MvxFormsWindowsPage : FormsApplicationPage
    {
        public MvxFormsWindowsPage()
        {
            //    // Wait for page to load to kick off setup and loading forms 
            //    // This is required for when setup becomes async aware
            //    Loaded += MvxFormsWindowsPage_Loaded;
            //}

            //private void MvxFormsWindowsPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
            //{
            //    Loaded -= MvxFormsWindowsPage_Loaded;

            Initialized += MvxWindow_Initialized;
        }

        private void MvxWindow_Initialized(object sender, EventArgs e)
        {
            //    if (this == System.Windows.Application.Current.MainWindow)
            //    {
            //        (System.Windows.Application.Current as MvvmCross.Platforms.Wpf.Views.MvxApplication).ApplicationInitialized();
            //    }

            //    LoadFormsApplication();
            RunAppStart(e);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            MvxWpfSetupSingleton.EnsureSingletonAvailable(Dispatcher, this).EnsureInitialized();

            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
                startup.Start(GetAppStartHint(hint));

            LoadFormsApplication();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
        }

        protected virtual void LoadFormsApplication()
        {
            var presenter = Mvx.IoCProvider.Resolve<IMvxFormsViewPresenter>() as MvxFormsWpfViewPresenter;
            LoadApplication(presenter.FormsApplication);
        }
    }
}
