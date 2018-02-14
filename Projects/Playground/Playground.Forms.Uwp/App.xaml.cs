using MvvmCross.Platform.Uap.Core;
using Playground.Forms.UI;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using MvvmCross.Forms.Platform.Uap.Core;
using MvvmCross.Forms.Views.Base;

namespace Playground.Forms.Uwp
{
    sealed partial class App 
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override MvxWindowsSetup CreateSetup(Frame rootFrame, IActivatedEventArgs e, string suspension)
        {
            return new MvxFormsWindowsSetup<Core.App,FormsApp>(rootFrame, e, suspension);
        }
    }
}
