using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using CustomerManagement.Core.Interfaces;
using Microsoft.Phone.Controls;

namespace CustomerManagement.WindowsPhone
{
    public class Setup 
        : MvxPhoneSetup
    {
        private readonly IViewModelCloser _closer;

        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
            _closer = new ViewModelCloser(rootFrame);
        }

        protected override IMvxApplication CreateApp()
        {
            var app = new Core.App();
            return app;
        }

        protected override void InitializeLastChance()
        {
            Mvx.RegisterSingleton<IViewModelCloser>(_closer);
            base.InitializeLastChance();
        }
    }
}
