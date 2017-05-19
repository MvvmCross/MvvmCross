using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using Xamarin.Forms;
using XamlControls = Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using MvvmCross.Binding;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Forms.Uwp
{
    public abstract class MvxFormsWindowsSetup : MvxWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;

        public MvxFormsWindowsSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            Xamarin.Forms.Forms.Init(_launchActivatedEventArgs);

            var xamarinFormsApp = new MvxFormsApplication();
            var presenter = new MvxFormsUwpPagePresenter(rootFrame, xamarinFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }

        protected override void InitializeLastChance()
        {
            InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitializeBindingBuilder()
        {
            MvxBindingBuilder bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxFormsBindingBuilder();
        }
    }
}
