using Windows.UI.Xaml.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Logging;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;

namespace Playground.Uwp
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame, string suspensionManagerSessionStateKey = null) : base(rootFrame, suspensionManagerSessionStateKey)
        {
        }

        public Setup(IMvxWindowsFrame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxLogProvider CreateLogProvider()
        {
            return new EmptyVoidLogProvider();
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
    }
}