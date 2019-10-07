﻿using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;
using Playground.Forms.UI;

namespace Playground.Forms.WpfCore
{
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxFormsWpfSetup<Core.App, FormsApp>>();
        }
    }
}
