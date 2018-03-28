using MvvmCross.Core;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;

namespace Playground.Uwp
{
    public sealed partial class App : MvxApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWindowsSetup<Core.App>>();
        }
    }
}
