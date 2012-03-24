using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ViewModels;
using TwitterSearch.Core.Interfaces;
using TwitterSearch.Core.Models;

namespace TwitterSearch.Core.ViewModels
{
    public class TwitterViewModel
        : MvxViewModel
        , IMvxServiceConsumer<ITwitterSearchProvider>
    {
        public TwitterViewModel(string searchTerm)
        {
            StartSearch(searchTerm);
        }

        private bool _isSearching;
        public bool IsSearching
        {
            get { return _isSearching; }
            set { _isSearching = value; FirePropertyChanged("IsSearching"); }
        }

        private IEnumerable<Tweet> _tweets;
        public IEnumerable<Tweet> Tweets
        {
            get { return _tweets; }
            set { _tweets = value; FirePropertyChanged("Tweets"); }
        }

        private ITwitterSearchProvider TwitterSearchProvider
        {
            get { return this.GetService<ITwitterSearchProvider>(); }
        }

        private void StartSearch(string searchTerm)
        {
            if (IsSearching)
                return;

            IsSearching = true;
            TwitterSearchProvider.StartAsyncSearch(searchTerm, Success, Error);
        }

        private void Error(Exception exception)
        {
            // for now we just hide the error...
            IsSearching = false;
        }

        private void Success(IEnumerable<Tweet> enumerable)
        {
            InvokeOnMainThread(() => DisplayTweets(enumerable));
        }

        private void DisplayTweets(IEnumerable<Tweet> enumerable)
        {
            IsSearching = false;
            Tweets = enumerable.ToList();
        }
    }
}