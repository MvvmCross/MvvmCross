using System.Windows;
using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;
using Playground.Forms.UI;

namespace Playground.Forms.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxFormsWpfSetup<Core.App, FormsApp>>();
        }
    }
}
