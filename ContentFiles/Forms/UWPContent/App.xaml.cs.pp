using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.Forms.Platforms.Uap.Views;

namespace $rootnamespace$
{
    sealed partial class App
    {
        public App()
        {
            this.InitializeComponent();
        }
    }

    public abstract class UWPApplication : MvxWindowsApplication<MvxFormsWindowsSetup<Core.App, FormsUI.App>, Core.App, FormsUI.App, MainPage>
    {
    }
}