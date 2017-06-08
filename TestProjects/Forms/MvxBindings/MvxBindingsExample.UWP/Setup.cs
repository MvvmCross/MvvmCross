using Windows.ApplicationModel.Activation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Uwp;
using MvvmCross.Platform.Platform;
using XamlControls = Windows.UI.Xaml.Controls;
using ExampleApp = MvxBindingsExample.App;

namespace MvxBindingsExample.UWP
{
    public class Setup : MvxFormsWindowsSetup
    {
        public Setup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame, e)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new ExampleApp();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}