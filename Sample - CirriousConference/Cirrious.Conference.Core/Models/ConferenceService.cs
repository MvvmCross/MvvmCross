using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.Conference.Core.Models.Raw;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cirrious.Conference.Core.Models
{
    public class ConferenceService 
        : IConferenceService
        
    {
        private readonly FavoritesSaver _favoritesSaver = new FavoritesSaver();

        // is loading setup
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set 
            { 
                _isLoading = value;
                FireLoadingChanged();
            }
        }

        private void FireLoadingChanged()
        {
			FireMessage(new LoadingChangedMessage(this));
        }
 
        // the basic lists
        public IDictionary<string, SessionWithFavoriteFlag> Sessions { get; private set; }
        public IDictionary<string, Sponsor> Exhibitors { get; private set; }
        public IDictionary<string, Sponsor> Sponsors { get; private set; }

        // a hashtable of favorites
        private IDictionary<string, SessionWithFavoriteFlag> _favoriteSessions;
        public IDictionary<string,SessionWithFavoriteFlag> GetCopyOfFavoriteSessions()
        {
            lock (this)
            {
				if (_favoriteSessions == null)
					return new Dictionary<string, SessionWithFavoriteFlag>();
				
                var toReturn = new Dictionary<string, SessionWithFavoriteFlag>(_favoriteSessions);
                return toReturn;
            }
        }
		 
        private void FireFavoriteSessionsChanged()
        {
			FireMessage(new FavoritesChangedMessage(this));
        }

		private void FireMessage(MvxMessage message)
		{
			var messenger = Mvx.Resolve<IMvxMessenger>();
			messenger.Publish(message);
		}

        public void BeginAsyncLoad()
        {
            IsLoading = true;
            MvxAsyncDispatcher.BeginAsync(Load);
        }
		
		public void DoSyncLoad()
        {
            IsLoading = true;
            Load();
        }

        private void Load()
        {
            LoadSessions();
            LoadFavorites();
            LoadSponsors();

            IsLoading = false;
        }		
		
        private void LoadSponsors()
        {
            var file = Mvx.Resolve<IMvxResourceLoader>().GetTextResource("ConfResources/Sponsors.txt");
            var jsonConvert = Mvx.Resolve<IMvxJsonConverter>();
            var items = jsonConvert.DeserializeObject<List<Sponsor>>(file);
            Sponsors = items.Where(x => x.Level != "Exhibitor").ToDictionary(x => x.Name);
            Exhibitors = items.Where(x => x.Level == "Exhibitor").ToDictionary(x => x.Name);
        }

        private void LoadFavorites()
        {
            lock (this)
            {
                _favoriteSessions = new Dictionary<string, SessionWithFavoriteFlag>();
            }
            FireFavoriteSessionsChanged();

            var files = Mvx.Resolve<IMvxFileStore>();
            string json;
            if (!files.TryReadTextFile(Constants.FavoritesFileName, out json))
                return;

            var jsonConvert = Mvx.Resolve<IMvxJsonConverter>();
            var parsedKeys = jsonConvert.DeserializeObject<List<string>>(json);
            if (parsedKeys != null)
            {
                foreach (var key in parsedKeys)
                {
                    SessionWithFavoriteFlag session;
                    if (Sessions.TryGetValue(key, out session))
                        session.IsFavorite = true;
                }
            }
        }

        private void LoadSessions()
        {
            var file = Mvx.Resolve<IMvxResourceLoader>().GetTextResource("ConfResources/Sessions.txt");
            var jsonConvert = Mvx.Resolve<IMvxJsonConverter>();
            var items = jsonConvert.DeserializeObject<List<Session>>(file);
            foreach (var item in items)
            {
                item.Key = item.Title;
            }
            Sessions = items.Select(x => new SessionWithFavoriteFlag()
                                                  {
                                                      Session = x,
                                                      IsFavorite = false
                                                  })
                .ToDictionary(x => x.Session.Key, x => x);

            foreach (var sessionWithFavoriteFlag in Sessions.Values)
            {
                sessionWithFavoriteFlag.PropertyChanged += SessionWithFavoriteFlagOnPropertyChanged;            
            }
        }

        private void SessionWithFavoriteFlagOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "IsFavorite")
                return;

            var session = (SessionWithFavoriteFlag)sender;
            lock (this)
            {
                if (_favoriteSessions == null)
                    return;

                if (session.IsFavorite)
                {
                    _favoriteSessions[session.Session.Key] = session;
                }
                else
                {
                    if (_favoriteSessions.ContainsKey(session.Session.Key))
                        _favoriteSessions.Remove(session.Session.Key);
                }

                _favoritesSaver.RequestAsyncSave(_favoriteSessions.Keys.ToList());
            }

            FireFavoriteSessionsChanged();
        }
    }
}
