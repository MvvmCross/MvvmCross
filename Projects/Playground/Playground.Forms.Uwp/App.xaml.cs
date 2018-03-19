using System;
using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Core;
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
