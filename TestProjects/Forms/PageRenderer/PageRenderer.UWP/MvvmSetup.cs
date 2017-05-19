using Windows.ApplicationModel.Activation;
using XamlControls = Windows.UI.Xaml.Controls;

using Xamarin.Forms;

using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Uwp;
using MvvmCross.Forms.Uwp.Presenters;
using PageRendererExample.ViewModels;

namespace PageRendererExample.UI.Uwp
{
    class MvvmSetup : MvxFormsWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;
        public MvvmSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame, e)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.LazyConstructAndRegisterSingleton<IImageHolder, ImageHolder>();
            return new MvvmApp();
        }
    }
}
