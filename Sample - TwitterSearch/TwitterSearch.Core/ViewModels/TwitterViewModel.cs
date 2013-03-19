using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using TwitterSearch.Core.Interfaces;
using TwitterSearch.Core.Models;

namespace TwitterSearch.Core.ViewModels
{
    public class TwitterViewModel
        : MvxViewModel        
    {
        public TwitterViewModel(ITwitterSearchProvider searchProvider)
        {
            TwitterSearchProvider = searchProvider;
        }
        
        public void Init(string searchTerm)
        {
            StartSearch(searchTerm);
        }

        private bool _isSearching;
        public bool IsSearching
        {
            get { return _isSearching; }
            set { _isSearching = value; RaisePropertyChanged("IsSearching"); }
        }

        private IEnumerable<Tweet> _tweets;
        public IEnumerable<Tweet> Tweets
        {
            get { return _tweets; }
            set { _tweets = value; RaisePropertyChanged("Tweets"); }
        }

        private ITwitterSearchProvider TwitterSearchProvider { get; set; }

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