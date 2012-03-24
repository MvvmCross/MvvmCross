using System;
using System.Linq;

namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public class SessionListViewModel
        : BaseSessionListViewModel<DateTime>
    {       
        public SessionListViewModel(string day)
        {
#warning - TODO - this is a bit hacky...
            int dayOfMonth;
            switch (day)
            {
                case "Thursday":
                    dayOfMonth = 29;
                    break;
                case "Friday":
                    dayOfMonth = 30;
                    break;
                case "Saturday":
                default:
                    dayOfMonth = 31;
                    break;
            }

            var grouped = Service.Sessions
                .Values
                .Where(slot => slot.Session.When.Day == dayOfMonth)
                .GroupBy(slot => slot.Session.When)
                .OrderBy(slot => slot.Key)
                .Select(slot => new SessionGroup(
                                    slot.Key,
                                    slot.OrderBy(session => session.Session.Title),
                                    NavigateToSession));

            Title = day;
            GroupedList = grouped.ToList();
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; FirePropertyChanged("Title"); }
        }
    }
}