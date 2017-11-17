using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Uwp;
using MvvmCross.Platform.Logging;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Playground.Forms.Uwp
{
    public class Setup : MvxFormsWindowsSetup<FormsApp>
    {
        public Setup(Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame, e)
        {
        }

        protected override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.None;

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
    }
}
