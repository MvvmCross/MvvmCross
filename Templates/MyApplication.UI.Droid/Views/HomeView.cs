
using Android.App;
using Cirrious.MvvmCross.Binding.Droid.Views;
using MyApplication.Core.ViewModels;

namespace MyApplication.UI.Droid.Views
{
    [Activity]
    public class HomeView
        : MvxBindingActivityView<HomeViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_HomeView);
        }
    }
}