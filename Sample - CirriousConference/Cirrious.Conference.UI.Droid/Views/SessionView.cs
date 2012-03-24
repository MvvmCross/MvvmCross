using Android.App;
using Cirrious.Conference.Core.ViewModels;

namespace Cirrious.Conference.UI.Droid.Views
{
    [Activity]
    public class SessionView : BaseView<SessionViewModel>
    {
        protected override void OnViewModelSet()
        {
            this.SetContentView(Resource.Layout.Page_Session);
        }
    }
}