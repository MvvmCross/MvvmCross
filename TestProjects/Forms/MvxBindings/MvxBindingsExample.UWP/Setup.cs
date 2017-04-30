using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using Xamarin.Forms;
using XamlControls = Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.IoC;
using ExampleApp = MvxBindingsExample.App;

namespace MvxBindingsExample.UWP
{
    public class HackMvxStoreResourceLoader : MvxStoreResourceLoader
    {
        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            // in 3.0.8.2 and earlier we needed to replace the "/" with "\\" :/
            resourcePath = resourcePath.Replace("/", "\\");
            StorageFolder install = Package.Current.InstalledLocation;
            base.GetResourceStream(resourcePath, streamAction);
        }
    }

    public class Setup : MvxWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;

        public Setup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.RegisterType<IMvxResourceLoader, HackMvxStoreResourceLoader>();
            return new ExampleApp();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            Forms.Init(_launchActivatedEventArgs);

            var xamarinFormsApp = new MvxFormsApp();
            var presenter = new MvxFormsUwpPagePresenter(  rootFrame, xamarinFormsApp);
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