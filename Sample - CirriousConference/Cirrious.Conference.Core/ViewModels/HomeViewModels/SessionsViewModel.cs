using System.Windows.Input;
using Cirrious.Conference.Core.ViewModels.SessionLists;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.Conference.Core.ViewModels.HomeViewModels
{
    public class SessionsViewModel
        : BaseConferenceViewModel
    {        
        public ICommand ShowExhibitorsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<ExhibitionViewModel>()); }
        }

        public ICommand ShowTopicsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<TopicsViewModel>()); }
        }

        public ICommand ShowSpeakersCommand    
        {
            get { return new MvxRelayCommand(() => RequestNavigate<SpeakersViewModel>()); }
        }

        public ICommand ShowDayCommand
        {
            get { return new MvxRelayCommand<string>((day) => RequestNavigate<SessionListViewModel>(new {day = day})); }
        }

        public ICommand ShowThursdayCommand
        {
            get { return MakeDayCommand("Thursday"); }
        }

        public ICommand ShowFridayCommand
        {
            get { return MakeDayCommand("Friday"); }
        }

        public ICommand ShowSaturdayCommand
        {
            get { return MakeDayCommand("Saturday"); }
        }

        private ICommand MakeDayCommand(string whichDay)
        {
            return new MvxRelayCommand(() => RequestNavigate<SessionListViewModel>(new {day = whichDay}));
        }
    }
}