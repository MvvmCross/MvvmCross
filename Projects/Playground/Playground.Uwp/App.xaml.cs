using MvvmCross.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Views;
using Windows.UI.Xaml.Controls;

namespace Playground.Uwp
{
    sealed partial class App:MvxBaseApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override MvxWindowsSetup CreateSetup(Frame rootFrame, string suspension)
        {
            return new Setup(rootFrame, suspension);
        }
    }
}
