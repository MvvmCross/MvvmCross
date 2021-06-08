using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;

namespace Playground.Forms.WpfCore
{
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<Setup>();
        }
    }
}
