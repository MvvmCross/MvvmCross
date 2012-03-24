using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.Conference.Core.ViewModels.HomeViewModels
{
    public class WelcomeViewModel
        : BaseConferenceViewModel
    {
        public IMvxCommand ShowSponsorsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<SponsorsViewModel>()); }
        }
        public IMvxCommand ShowExhibitorsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<ExhibitionViewModel>()); }
        }
        public IMvxCommand ShowMapCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<MapViewModel>()); }
        }
        public IMvxCommand ShowAboutCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<AboutViewModel>()); }
        }
    }
}