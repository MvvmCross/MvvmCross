using MvvmCross.Core;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;

namespace Playground.Uwp
{
    public sealed partial class App : MvxApplication<MvxWindowsSetup<Core.App>, Core.App>
    {
        public App()
        {
            InitializeComponent();
        }
    }
}
