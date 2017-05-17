using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using Xamarin.Forms;
using XamlControls = Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using MvvmCross.Binding;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Uwp;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
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