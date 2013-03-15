using Android.App;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Droid.Views
{
    [Activity(Label = "TwitterSearch")]
    public class HomeView : MvxActivityView
    {
        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public HomeView()
        {
            MvxTrace.Trace("Constructo called");
        }

        protected override void OnCreate(Android.OS.Bundle bundle)        
        {
            MvxTrace.Trace("OnCreate called with {0}", bundle == null);
            base.OnCreate(bundle);
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Home);
        }

        protected override void OnSaveInstanceState(Android.OS.Bundle outState)
        {
            MvxTrace.Trace("SaveInstanceState called with {0}", outState == null);
            base.OnSaveInstanceState(outState);
        }
    }
}