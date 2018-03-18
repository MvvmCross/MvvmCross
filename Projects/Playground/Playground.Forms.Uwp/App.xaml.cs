using System;
using MvvmCross.Forms.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Core;
using Playground.Forms.UI;

namespace Playground.Forms.Uwp
{
    sealed partial class App
    {
        static App()
        {
            MvxWindowsSetup.RegisterWindowsSetupType<MvxFormsWindowsSetup<Core.App, FormsApp>>();
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
