using MvvmCross.Forms.Views.Base;
using MvvmCross.Platform.Uap.Views;
using Windows.ApplicationModel.Activation;

namespace MvvmCross.Forms.Platform.Uap.Views
{
    public abstract class MvxWindowsApplication : MvxApplication
    {
        protected override void Start(IActivatedEventArgs activationArgs)
        {
            if (RootFrame?.Content == null) {
                RootFrame.Navigate(typeof(MvxFormsWindowsPage), (activationArgs as LaunchActivatedEventArgs)?.Arguments);
            }
        }
    }
}
