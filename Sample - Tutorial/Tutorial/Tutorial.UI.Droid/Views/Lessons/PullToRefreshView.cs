
using Android.App;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.Droid.Views.Lessons
{
    [Activity]
    public class PullToRefreshView
        : MvxActivity
    {
        public new PullToRefreshViewModel ViewModel
        {
            get { return (PullToRefreshViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_PullToRefreshView);
        }
    }
}