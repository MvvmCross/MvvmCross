﻿using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using Xamarin.Forms;
using XamlControls = Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;

namespace MasterDetailExample.UWP
{
    public class Setup : MvxWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;

        public Setup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            Forms.Init(_launchActivatedEventArgs);

            var xamarinFormsApp = new MvxFormsApplication();
            var presenter = new MvxFormsUwpMasterDetailPagePresenter(rootFrame, xamarinFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }
    }
}