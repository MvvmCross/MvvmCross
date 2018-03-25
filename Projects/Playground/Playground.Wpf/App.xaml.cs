using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;

namespace Playground.Wpf
{
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            MvxSetup.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }
    }
}
