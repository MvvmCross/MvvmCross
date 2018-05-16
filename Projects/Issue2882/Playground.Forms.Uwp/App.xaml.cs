using MvvmCross.Forms.Platforms.Uap.Views;
using Playground.Forms.UI;

namespace Playground.Forms.Uwp
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
        }
    }

    public abstract class PlaygroundApp : MvxWindowsApplication<Setup, Core.App, FormsApp, MainPage>
    {
    }
}
