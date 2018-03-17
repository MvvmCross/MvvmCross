using MvvmCross.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Views;

namespace Playground.Uwp
{
    public sealed partial class App : MvxApplication
    {
        static App()
        {
            MvxWindowsSetup.RegisterWindowsSetupType<MvxWindowsSetup<Core.App>>();
        }

        public App()
        {
            InitializeComponent();
        }
    }
}
