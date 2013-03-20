using Cirrious.CrossCore.IoC;
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

        protected override MvxApplication CreateApp()
        {
            var app = new CustomerManagement.AutoViews.Core.App();
            return app;
        }

        protected override IMvxNavigationRequestSerializer CreateNavigationRequestSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
            var json = Mvx.Resolve<IMvxJsonConverter>();
            return new MvxNavigationRequestSerializer(json);
        }

        protected override void InitializeLastChance()
        {
            Mvx.RegisterSingleton<IViewModelCloser>(_closer);
            base.InitializeLastChance();
        }
    }
}
