
using Android.App;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using MyApplication.Core.ViewModels;

namespace MyApplication.UI.Droid.Views
{
    [Activity]
    public class HomeView
        : MvxActivityView
    {
        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel)base.ViewModel;  }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_HomeView);
        }
    }
}