using MvvmCross.Platforms.Uap.Core;
using MvvmCross.ViewModels;
using Windows.UI.Xaml.Controls;

namespace $rootnamespace$
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
    }
}
