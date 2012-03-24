using Android.App;
using Cirrious.Conference.Core.ViewModels;

namespace Cirrious.Conference.UI.Droid.Views
{
    [Activity]
    public class AboutView : BaseView<AboutViewModel>
    {
        protected override void OnViewModelSet()
        {
            this.SetContentView(Resource.Layout.Page_About);
        }
    }
}