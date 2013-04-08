using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace Tutorial.UI.WindowsPhone
{
    public class Setup
        : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            var app = new Core.App();
            return app;
        }

        protected override void InitializeLastChance()
        {
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();

            base.InitializeLastChance();
        }
    }
}
