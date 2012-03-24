using Android.App;
using Cirrious.MvvmCross.Binding.Android.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Droid.Views
{
    [Activity(Label = "TwitterSearch")]
    public class TwitterView : MvxBindingActivityView<TwitterViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Twitter);
        }
    }
}