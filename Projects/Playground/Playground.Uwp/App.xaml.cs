using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;

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
