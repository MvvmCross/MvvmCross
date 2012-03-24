using Android.App;
using Cirrious.Conference.Core.ViewModels.HomeViewModels;

namespace Cirrious.Conference.UI.Droid.Views.Home
{
    [Activity]
    public class TwitterView : BaseView<TwitterViewModel>
    {
        protected override void OnViewModelSet()
        {
            this.SetContentView(Resource.Layout.ChildPage_Twitter);
        }
    }
}