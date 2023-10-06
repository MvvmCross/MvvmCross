using Microsoft.UI.Xaml;
using MvvmCross.Core;
using MvvmCross.Platforms.WinUi.Views;
using Playground.WinUi3;

namespace Playground.WinUi
{
    public sealed partial class App : MvxApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow()
        {
            return new Window()
            {
                Title = "MvvmCross WinUI 3 Playground"
            };
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<WinUiPlaygroundSetup>();
        }
    }
}
