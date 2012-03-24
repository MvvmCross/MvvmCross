using Cirrious.Conference.Core.ViewModels.SessionLists;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.Conference.Core.ViewModels.HomeViewModels
{
    public class SessionsViewModel
        : BaseConferenceViewModel
    {        public IMvxCommand ShowExhibitorsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<ExhibitionViewModel>()); }
        }

        public IMvxCommand ShowTopicsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<TopicsViewModel>()); }
        }
        public IMvxCommand ShowSpeakersCommand    
        {
            get { return new MvxRelayCommand(() => RequestNavigate<SpeakersViewModel>()); }
        }
        public IMvxCommand ShowDayCommand
        {
            get { return new MvxRelayCommand<string>((day) => RequestNavigate<SessionListViewModel>(new {day = day})); }
        }

        public IMvxCommand ShowThursdayCommand
        {
            get { return MakeDayCommand("Thursday"); }
        }

        public IMvxCommand ShowFridayCommand
        {
            get { return MakeDayCommand("Friday"); }
        }

        public IMvxCommand ShowSaturdayCommand
        {
            get { return MakeDayCommand("Saturday"); }
        }

        private IMvxCommand MakeDayCommand(string whichDay)
        {
            return new MvxRelayCommand(() => RequestNavigate<SessionListViewModel>(new {day = whichDay}));
        }
    }
}