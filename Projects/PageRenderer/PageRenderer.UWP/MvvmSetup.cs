using Windows.ApplicationModel.Activation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Uwp;
using MvvmCross.Platform;
using XamlControls = Windows.UI.Xaml.Controls;

namespace PageRendererExample.WindowsUWP
{
    internal class MvvmSetup : MvxFormsWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;
        public MvvmSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame, e)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.LazyConstructAndRegisterSingleton<IImageHolder, ImageHolder>();
            return new CoreApp();
        }
    }
}
