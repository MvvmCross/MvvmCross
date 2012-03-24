using System;
using System.Linq;

namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public class FavoritesViewModel
        : BaseSessionListViewModel<DateTime>
    {
        public FavoritesViewModel()
        {
            RebuildFavorites();
            Service.FavoritesSessionsChanged += ServiceOnFavoritesSessionsChanged;
        }

        public override void OnViewsDetached()
        {
            Service.FavoritesSessionsChanged -= ServiceOnFavoritesSessionsChanged;
            base.OnViewsDetached();
        }

        private void ServiceOnFavoritesSessionsChanged(object sender, EventArgs eventArgs)
        {
            InvokeOnMainThread(RebuildFavorites);
        }

        private void RebuildFavorites()
        {
            var grouped = Service.GetCopyOfFavoriteSessions()
                .Values
                .GroupBy(slot => slot.Session.When)
                .OrderBy(slot => slot.Key)
                .Select(slot => new SessionGroup(
                                    slot.Key,
                                    slot.OrderBy(session => session.Session.Title),
                                    NavigateToSession));

            GroupedList = grouped.ToList();
        }
    }
}