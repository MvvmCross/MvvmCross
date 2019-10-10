using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Logging;
using MvvmCross.Uwp.Platform;
using Windows.UI.Xaml.Controls;

namespace TipCalc.UWP
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

        protected override MvxLogProviderType GetDefaultLogProviderType()
        {
            return MvxLogProviderType.None;
        }
    }
}
