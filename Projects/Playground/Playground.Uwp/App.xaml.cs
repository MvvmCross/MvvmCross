using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;

namespace Playground.Uwp
{
    public sealed partial class App
    {
        public App()
        {
            InitializeComponent();
        }
    }

    public abstract class PlaygroundApp : MvxApplication<MvxWindowsSetup<Core.App>, Core.App>
    {
    }
}
