using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.Conference.UI.Touch.Views
{
    public class ExhibitorsView
        : BaseSponsorsView<ExhibitionViewModel>
    {
        public ExhibitorsView(MvxShowViewModelRequest request)
            : base(request)
        {
        }
    }
}