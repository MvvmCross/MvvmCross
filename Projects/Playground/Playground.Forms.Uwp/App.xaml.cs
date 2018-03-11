using System;
using MvvmCross.Forms.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Core;
using Playground.Forms.UI;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Playground.Forms.Uwp
{
    sealed partial class App 
    {
        public App()
        {
            InitializeComponent();
        }

        protected override MvxWindowsSetup CreateSetup(Frame rootFrame, IActivatedEventArgs e, string suspension)
        {
            return new MvxFormsWindowsSetup<Core.App, FormsApp>(rootFrame, e, suspension);
        }

        protected override Type HostWindowsPageType()
        {
            return typeof(MainPage);
        }
    }
}
