using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;
using Tutorial.Core.ViewModels;
using Tutorial.Core.ViewModels.Lessons;
using Tutorial.UI.WindowsPhone.Views;
using Tutorial.UI.WindowsPhone.Views.Lessons;

namespace Tutorial.UI.WindowsPhone
{
    public class Setup
        : MvxBaseWindowsPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app = new Core.App();
            return app;
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return new Dictionary<Type, Type>()
                       {
                            { typeof(MainMenuViewModel), typeof(MainMenuView)},
                            { typeof(SimpleTextPropertyViewModel), typeof(SimpleTextPropertyView)},
                            { typeof(PullToRefreshViewModel), typeof(PullDownToRefreshView)},
                       };
        }
    }
}
