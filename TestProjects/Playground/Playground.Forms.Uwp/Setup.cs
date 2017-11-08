using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Uwp;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Platform;
using MvvmCross.Uwp.Views;
using XamlControls = Windows.UI.Xaml.Controls;
using MvvmCross.Platform.Logging;
using Windows.ApplicationModel.Activation;
using XamlControls = Windows.UI.Xaml.Controls;

namespace Playground.Forms.Uwp
{
    public class Setup : MvxFormsWindowsSetup
    {
        public Setup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame, e)
        {
        }

        protected override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.None;

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override MvxFormsApplication CreateFormsApplication()
        {
            return new Core.FormsApp();
        }
    }
}
