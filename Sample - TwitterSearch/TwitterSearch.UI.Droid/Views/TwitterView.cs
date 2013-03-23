using Android.App;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Droid.Views
{
    [Activity(Label = "TwitterSearch")]
    public class TwitterView : MvxActivity
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Twitter);
        }
    }
}