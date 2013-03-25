using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ViewModels.HomeViewModels
{
    public class WelcomeViewModel
        : BaseConferenceViewModel
    {
        public ICommand ShowSponsorsCommand
        {
            get { return new MvxCommand(() => ShowViewModel<SponsorsViewModel>()); }
        }

        public ICommand ShowExhibitorsCommand
        {
            get { return new MvxCommand(() => ShowViewModel<ExhibitionViewModel>()); }
        }
        
        public ICommand ShowMapCommand
        {
            get { return new MvxCommand(() => ShowViewModel<MapViewModel>()); }
        }
        
        public ICommand ShowAboutCommand
        {
            get { return new MvxCommand(() => ShowViewModel<AboutViewModel>()); }
        }
    }
}