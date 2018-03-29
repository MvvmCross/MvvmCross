using MvvmCross.Core;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;

namespace Playground.Uwp
{
    public sealed partial class App : PlaygroundApp
    {
        public App()
        {
            InitializeComponent();
        }
    }

    public class PlaygroundApp : MvxApplication<MvxWindowsSetup<Core.App>, Core.App>
    {

    }
}
