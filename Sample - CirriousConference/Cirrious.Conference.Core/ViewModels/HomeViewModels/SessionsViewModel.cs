using System.Windows.Input;
using Cirrious.Conference.Core.ViewModels.SessionLists;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ViewModels.HomeViewModels
{
    public class SessionsViewModel
        : BaseConferenceViewModel
    {
        public ICommand ShowExhibitorsCommand
        {
            get { return new MvxRelayCommand(() => ShowViewModel<ExhibitionViewModel>()); }
        }

        public ICommand ShowTopicsCommand
        {
            get { return new MvxRelayCommand(() => ShowViewModel<TopicsViewModel>()); }
        }

        public ICommand ShowSpeakersCommand    
        {
            get { return new MvxRelayCommand(() => ShowViewModel<SpeakersViewModel>()); }
        }

        public ICommand ShowDayCommand
        {
            get { return new MvxRelayCommand<string>((day) => ShowViewModel<SessionListViewModel>(new {dayOfMonth = int.Parse(day)})); }
        }

        public ICommand ShowThursdayCommand
        {
            get { return MakeDayCommand(29); }
        }

        public ICommand ShowFridayCommand
        {
            get { return MakeDayCommand(30); }
        }

        public ICommand ShowSaturdayCommand
        {
            get { return MakeDayCommand(31); }
        }

        private ICommand MakeDayCommand(int whichDayOfMonth)
        {
            return new MvxRelayCommand(() => ShowViewModel<SessionListViewModel>(new { dayOfMonth = whichDayOfMonth }));
        }
    }
}