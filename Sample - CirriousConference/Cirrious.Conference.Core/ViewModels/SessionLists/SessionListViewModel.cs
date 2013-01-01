using System;
using System.Linq;

namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public class SessionListViewModel
        : BaseSessionListViewModel<DateTime>
    {       
        public SessionListViewModel(int dayOfMonth)
        {
            var grouped = Service.Sessions
                .Values
                .Where(slot => slot.Session.When.Day == dayOfMonth)
                .GroupBy(slot => slot.Session.When)
                .OrderBy(slot => slot.Key)
                .Select(slot => new SessionGroup(
                                    slot.Key,
                                    slot.OrderBy(session => session.Session.Title),
                                    NavigateToSession));

            var day = DayFrom(dayOfMonth);
            Title = day;
            GroupedList = grouped.ToList();
        }

        private static string DayFrom(int dayOfMonth)
        {
            string day;
            switch (dayOfMonth)
            {
                case 29:
                    day = "Thursday";
                    break;
                case 30:
                    day = "Friday";
                    break;
                case 31:
                default:
                    day = "Saturday";
                    break;
            }
            return day;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged("Title"); }
        }
    }
}