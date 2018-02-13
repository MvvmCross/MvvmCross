using MvvmCross.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Playground.Uwp
{
    public sealed partial class App : MvxApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override MvxWindowsSetup CreateSetup(Frame rootFrame, IActivatedEventArgs activatedEventArgs, string suspension)
        {
            return new MvxWindowsSetup<Core.App>(rootFrame, activatedEventArgs, suspension);
        }
    }
}
