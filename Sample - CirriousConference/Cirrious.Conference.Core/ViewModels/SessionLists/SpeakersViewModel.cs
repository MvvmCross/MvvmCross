using System.Linq;

namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public class SpeakersViewModel
        : BaseReloadingSessionListViewModel<string>
    {
        protected override void LoadSessions()
        {
            if (Service.Sessions == null)
                return;

            var grouped = Service.Sessions
                .Values
                .GroupBy(session => session.Session.SpeakerKey)
                .OrderBy(slot => slot.Key)
                .Select(slot => new SessionGroup(
                                slot.Key,
                                slot
                                    .OrderBy(session => session.Session.When),
                                NavigateToSession));

            GroupedList = grouped.ToList();
        }
    }
}