using Windows.UI.Xaml.Controls;
using MvvmCross.ViewModels;
using MvvmCross.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Views;

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

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
    }
}
