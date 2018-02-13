using MvvmCross.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Views;
using Windows.UI.Xaml.Controls;

namespace Playground.Uwp
{
    public sealed partial class App : MvxBaseApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override MvxWindowsSetup CreateSetup(Frame rootFrame, string suspension)
        {
            return new MvxTypedWindowsSetup<Core.App>(rootFrame, suspension);
        }
    }
}
