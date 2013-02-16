using System;
using System.Linq;

namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public class FavoritesViewModel
        : BaseSessionListViewModel<DateTime>
    {
		private Guid _subscription;
		
		public FavoritesViewModel()
        {
            RebuildFavorites();

			_subscription = Subscribe<FavoritesChangedMessage>(message => ServiceOnFavoritesSessionsChanged());
		}

        public override void OnViewsDetached()
        {
			if (_subscription != Guid.Empty) {
				Unsubscribe<FavoritesChangedMessage> (_subscription);
				_subscription = Guid.Empty;
			}

			base.OnViewsDetached();
        }

        private void ServiceOnFavoritesSessionsChanged()
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