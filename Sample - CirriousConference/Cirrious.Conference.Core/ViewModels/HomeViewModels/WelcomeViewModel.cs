using System.Windows.Input;
using Cirrious.CrossCore.Commands;

namespace Cirrious.Conference.Core.ViewModels.HomeViewModels
{
    public class WelcomeViewModel
        : BaseConferenceViewModel
    {
        public ICommand ShowSponsorsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<SponsorsViewModel>()); }
        }

        public ICommand ShowExhibitorsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<ExhibitionViewModel>()); }
        }
        
        public ICommand ShowMapCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<MapViewModel>()); }
        }
        
        public ICommand ShowAboutCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<AboutViewModel>()); }
        }
    }
}