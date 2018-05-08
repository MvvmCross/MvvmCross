// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Threading.Tasks;
using MvvmCross.Forms.Platforms.Wpf.Presenters;
using MvvmCross.Forms.Presenters;
using MvvmCross.ViewModels;
using Xamarin.Forms.Platform.WPF;
using MvvmCross.Platforms.Wpf.Core;

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

            var startup = Mvx.Resolve<IMvxAppStart>();
            if(!startup.IsStarted)
                startup.Start(GetAppStartHint(hint));

            LoadFormsApplication();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
        }

        protected virtual void LoadFormsApplication()
        {
            var presenter = Mvx.Resolve<IMvxFormsViewPresenter>() as MvxFormsWpfViewPresenter;
            LoadApplication(presenter.FormsApplication);
        }
    }
}
