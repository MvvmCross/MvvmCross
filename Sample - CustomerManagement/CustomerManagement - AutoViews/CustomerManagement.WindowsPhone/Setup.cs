using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using CustomerManagement.AutoViews.Core.Interfaces;
using CustomerManagement.WindowsPhone;
using Microsoft.Phone.Controls;

namespace CustomerManagement.AutoViews.WindowsPhone
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
            var app = new CustomerManagement.AutoViews.Core.App();
            return app;
        }

        protected override IMvxNavigationSerializer CreateNavigationSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
            return new MvxJsonNavigationSerializer();
        }

        protected override void InitializeLastChance()
        {
            Mvx.RegisterSingleton<IViewModelCloser>(_closer);
            base.InitializeLastChance();
        }
    }
}
