
using Android.App;
using Cirrious.MvvmCross.Binding.Android.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.Droid.Views.Lessons
{
    [Activity]
    public class TipView
        : MvxBindingActivityView<TipViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_TipView);
        }
    }
}