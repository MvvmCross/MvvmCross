
using Android.App;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.Droid.Views.Lessons
{
    [Activity]
    public class SimpleTextPropertyView
        : MvxActivity
    {
        public new SimpleTextPropertyViewModel ViewModel
        {
            get { return (SimpleTextPropertyViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_SimpleTextPropertyView);
        }
    }
}