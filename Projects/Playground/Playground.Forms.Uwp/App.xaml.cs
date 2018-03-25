using System;
using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Core;
using Playground.Forms.UI;

namespace Playground.Forms.Uwp
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Type HostWindowsPageType()
        {
            return typeof(MainPage);
        }

        protected override void RegisterSetup()
        {
            MvxSetup.RegisterSetupType<MvxFormsWindowsSetup<Core.App, FormsApp>>();
        }
    }
}
