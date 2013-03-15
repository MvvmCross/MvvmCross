using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WinRT.Platform;
using Windows.UI.Xaml.Controls;

namespace MyApplication.UI.WinRT
{
    public class Setup
        : MvxWinRTSetup
    {
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app = new MyApplication.Core.App();
            return app;
        }

        protected override void InitializeLastChance()
        {
            var errorDisplayer = new ErrorDisplayer();

            base.InitializeLastChance();
        }
    }
}
