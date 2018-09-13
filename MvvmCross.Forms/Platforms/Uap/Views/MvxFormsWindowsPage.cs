// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Platforms.Uap.Presenters;
using MvvmCross.Forms.Presenters;
using MvvmCross.ViewModels;
using Xamarin.Forms.Platform.UWP;

namespace MvvmCross.Forms.Platforms.Uap.Views
{
    public class MvxFormsWindowsPage : WindowsPage
    {
        public MvxFormsWindowsPage()
        {
            // This is required so that navigating to a native page and back again doesn't
            // reload XF
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

            // Wait for page to load to kick off setup and loading forms 
            // This is required for when setup becomes async aware
            Loaded += MvxWindowsPage_Loaded;
        }

        private void MvxWindowsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loaded -= MvxWindowsPage_Loaded;

            RunAppStart(e);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.IoCProvider.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(GetAppStartHint(hint));

            LoadFormsApplication();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
        }

        protected virtual void LoadFormsApplication()
        {
            var presenter = Mvx.IoCProvider.Resolve<IMvxFormsViewPresenter>() as MvxFormsUwpViewPresenter;
            LoadApplication(presenter.FormsApplication);
        }
    }
}
