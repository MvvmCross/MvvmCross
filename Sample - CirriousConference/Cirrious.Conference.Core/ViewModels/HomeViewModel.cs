using Cirrious.Conference.Core.ViewModels.HomeViewModels;
using Cirrious.Conference.Core.ViewModels.SessionLists;

namespace Cirrious.Conference.Core.ViewModels
{
    public class HomeViewModel
        : BaseConferenceViewModel
    {
        public HomeViewModel()
        {
            Welcome = new WelcomeViewModel();
            Sessions = new SessionsViewModel();            
            Twitter = new TwitterViewModel();
            Favorites = new FavoritesViewModel();
        }

        public override void OnViewsDetached()
        {
            Favorites.OnViewsDetached();
            Welcome.OnViewsDetached();
            Twitter.OnViewsDetached();
            Sessions.OnViewsDetached();

            base.OnViewsDetached();
        }

        public FavoritesViewModel Favorites { get; private set; }
        public WelcomeViewModel Welcome { get; private set; }
        public SessionsViewModel Sessions { get; private set; }
        public TwitterViewModel Twitter { get; private set; }
    }
}