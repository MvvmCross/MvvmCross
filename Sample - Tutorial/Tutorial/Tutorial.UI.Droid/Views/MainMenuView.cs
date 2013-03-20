
using Android.App;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using Tutorial.Core.ViewModels;

namespace Tutorial.UI.Droid.Views
{
    [Activity]
    public class MainMenuView
        : MvxActivity
    {
        public new MainMenuViewModel ViewModel
        {
            get { return (MainMenuViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_MainMenuView);
        }
    }
}