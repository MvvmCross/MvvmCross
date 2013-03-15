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

        public FavoritesViewModel Favorites { get; private set; }
        public WelcomeViewModel Welcome { get; private set; }
        public SessionsViewModel Sessions { get; private set; }
        public TwitterViewModel Twitter { get; private set; }
    }
}