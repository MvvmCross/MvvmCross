using MvvmCross.Forms.Platform.Uap.Presenters;
using MvvmCross.Forms.Platform.Uap.Views;
using MvvmCross.Forms.Presenters;
using MvvmCross.ViewModels;
using Xamarin.Forms.Platform.UWP;

namespace MvvmCross.Forms.Views.Base
{
    public class MvxFormsWindowsPage:WindowsPage
    {
        public MvxFormsWindowsPage()
        {
            // Wait for page to load to kick off setup and loading forms 
            // This is required for when setup becomes async aware
            Loaded += MvxWindowsPage_Loaded;
        }

        private void MvxWindowsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loaded -= MvxWindowsPage_Loaded;

            // This is required so that navigating to a native page and back again doesn't
            // reload XF
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

            StartSetup();

            LoadFormsApplication();
        }

        protected virtual void StartSetup()
        {
            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();
        }

        protected virtual void LoadFormsApplication()
        {
            var presenter = Mvx.Resolve<IMvxFormsViewPresenter>() as MvxFormsUwpViewPresenter;
            LoadApplication(presenter.FormsApplication);
        }
    }
}
