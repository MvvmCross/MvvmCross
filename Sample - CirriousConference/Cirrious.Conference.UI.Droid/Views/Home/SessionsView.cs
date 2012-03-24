using Android.App;
using Cirrious.Conference.Core.ViewModels.HomeViewModels;

namespace Cirrious.Conference.UI.Droid.Views.Home
{
    [Activity]
    public class SessionsView : BaseView<SessionsViewModel>
    {
        protected override void OnViewModelSet()
        {
            this.SetContentView(Resource.Layout.ChildPage_Sessions);
        }
    }
}