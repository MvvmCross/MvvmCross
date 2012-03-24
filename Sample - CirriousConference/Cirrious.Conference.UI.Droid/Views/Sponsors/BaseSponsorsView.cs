using Cirrious.Conference.Core.ViewModels;

namespace Cirrious.Conference.UI.Droid.Views.Sponsors
{
    public class BaseSponsorsView<TViewModel>
        : BaseView<TViewModel>
        where TViewModel : BaseSponsorsViewModel
    {
        protected sealed override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Sponsors);
        }
   }
}