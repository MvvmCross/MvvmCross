using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WinRT.Platform;
using Windows.UI.Xaml.Controls;

namespace MyApplication.UI.WinRT
{
    public class Setup
        : MvxBaseWinRTSetup
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

        protected override void AddPluginsLoaders(Cirrious.MvvmCross.Platform.MvxLoaderPluginRegistry loaders)
        {
            // provide loaders for any needed plugins here
            // e.g. loaders.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.WinRT.Plugin>();
            base.AddPluginsLoaders(loaders);
        }

        protected override void InitializeLastChance()
        {
            var errorDisplayer = new ErrorDisplayer();

            base.InitializeLastChance();
        }
    }
}
