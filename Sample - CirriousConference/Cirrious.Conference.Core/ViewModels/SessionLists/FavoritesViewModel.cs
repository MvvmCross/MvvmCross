using System;
using System.Linq;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public class FavoritesViewModel
        : BaseSessionListViewModel<DateTime>
    {
		private readonly MvxSubscriptionToken _mvxSubscription;
		
		public FavoritesViewModel()
        {
            RebuildFavorites();

			_mvxSubscription = Subscribe<FavoritesChangedMessage>(ServiceOnFavoritesSessionsChanged);
		}

        private void ServiceOnFavoritesSessionsChanged(FavoritesChangedMessage message)
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