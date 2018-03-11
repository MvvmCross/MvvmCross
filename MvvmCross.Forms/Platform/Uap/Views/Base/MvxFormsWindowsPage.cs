// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Forms.Platform.Uap.Presenters;
using MvvmCross.Forms.Platform.Uap.Views;
using MvvmCross.Forms.Presenters;
using MvvmCross.ViewModels;
using Xamarin.Forms.Platform.UWP;

namespace MvvmCross.Forms.Views.Base
{
    public class MvxFormsWindowsPage:WindowsPage
    {
        public MvxFormsWindowsPage()
        {
            // Wait for page to load to kick off setup and loading forms 
            // This is required for when setup becomes async aware
            Loaded += MvxWindowsPage_Loaded;
        }

        private async void MvxWindowsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loaded -= MvxWindowsPage_Loaded;

            // This is required so that navigating to a native page and back again doesn't
            // reload XF
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

            await InternalPageLoaded();
        }

        protected virtual async Task InternalPageLoaded()
        {
            await StartSetup();

            LoadFormsApplication();
        }


        protected virtual async Task StartSetup()
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            if (startup is IMvxAppStartAsync waitAppStart) {
                await waitAppStart.WaitForStart();
            }
        }

        protected virtual void LoadFormsApplication()
        {
            var presenter = Mvx.Resolve<IMvxFormsViewPresenter>() as MvxFormsUwpViewPresenter;
            LoadApplication(presenter.FormsApplication);
        }
    }
}
