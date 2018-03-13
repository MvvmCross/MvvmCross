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
        static App()
        {
            MvxWindowsSetup.RegisterSetupType< MvxFormsWindowsSetup < Core.App, FormsApp >> ();
        }

        public App()
        {
            InitializeComponent();
        }

        protected override Type HostWindowsPageType()
        {
            return typeof(MainPage);
        }
    }
}
