using MvvmCross.Core;
using MvvmCross.Platform.Wpf.Core;
using MvvmCross.Platform.Wpf.Views;

namespace Playground.Wpf
{
    public partial class App : MvxApplication
    {
        static App()
        {
            MvxSetup.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }
    }
}
