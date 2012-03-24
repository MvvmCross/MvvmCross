using Android.App;
using Cirrious.MvvmCross.Binding.Android.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Droid.Views
{
    [Activity(Label = "TwitterSearch")]
    public class SessionsView : MvxBindingActivityView<HomeViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Home);
        }
    }
}