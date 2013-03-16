using System.Collections.Generic;
using System.Linq;
using Cirrious.Conference.Core.Models;
using Cirrious.Conference.Core.Models.Raw;
using Cirrious.Conference.Core.ViewModels.Helpers;

namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public class TopicsViewModel
        : BaseReloadingSessionListViewModel<TopicsViewModel.TopicAndLevel>
    {
        public class TopicAndLevel
        {
            public string Topic { get; private set; }
            public string Level { get; private set; }

            public TopicAndLevel(Session session)
            {
                Topic = session.Type;
                Level = session.Level;
            }

            public override string ToString()
            {
                return string.Format("{0} ({1})", Topic, Level);
            }

            public override bool Equals(object obj)
            {
                var rhs = obj as TopicAndLevel;
                if (rhs == null)
                    return false;

                return rhs.Topic == Topic
                       && rhs.Level == Level;
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }

        protected override void LoadSessions()
        {
            if (Service.Sessions == null)
                return;

            var grouped = Service.Sessions
                .Values
                .GroupBy(slot => new TopicAndLevel(slot.Session))
                .OrderBy(slot => slot.Key.ToString())
                .Select(slot => new SessionGroup(
                                slot.Key,
                                slot.OrderBy(session => session.Session.When),
                                NavigateToSession));

            GroupedList = grouped.ToList();
        }
    }
}