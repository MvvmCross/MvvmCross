using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.Conference.Core.Models.Twitter;
using Cirrious.CrossCore.IoC;
using Cirrious.Conference.Core.ViewModels.Helpers;
using Cirrious.MvvmCross.Plugins.Network;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ViewModels.HomeViewModels
{
    public class TwitterViewModel
        : BaseViewModel
        
    {
        private const string SearchTerm = "SQLBits";

        private ITwitterSearchProvider TwitterSearchProvider
        {
            get { return Mvx.Resolve<ITwitterSearchProvider>(); }
        }

        private IEnumerable<Tweet> _tweets;
        public IEnumerable<Tweet> Tweets
        {
            get { return _tweets; }
            set { _tweets = value; RaisePropertyChanged("Tweets"); }
        }

        private IEnumerable<WithCommand<Tweet>> _tweetsPlus;
        public IEnumerable<WithCommand<Tweet>> TweetsPlus
        {
            get { return _tweetsPlus; }
            set { _tweetsPlus = value; RaisePropertyChanged("TweetsPlus"); }
        }
		
        private bool _isSearching;
        public bool IsSearching
        {
            get { return _isSearching; }
            set { _isSearching = value; RaisePropertyChanged("IsSearching"); }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new MvxCommand(StartSearch);
            }
        }
		
		public ICommand RefreshCommand
		{
			get
			{
                return new MvxCommand(StartSearch);
			}
		}
		
		private DateTime _whenLastUpdatedUtc = DateTime.MinValue;
		public DateTime WhenLastUpdatedUtc
		{
			get
			{
				return _whenLastUpdatedUtc;
			}
			set
			{
				_whenLastUpdatedUtc = value;
				RaisePropertyChanged("WhenLastUpdatedUtc");
			}
		}
		
        private void StartSearch()
        {
            if (IsSearching)
                return;
			
			IMvxReachability reach;
			if (Mvx.TryResolve<IMvxReachability>(out reach))
			{
				if (!reach.IsHostReachable("www.twitter.com"))
				{
				    ReportError(SharedTextSource.GetText("Error.NoNetwork"));
					return;
				}
			}
			
            IsSearching = true;
            TwitterSearchProvider.StartAsyncSearch(SearchTerm, Success, Error);
        }

        public TwitterViewModel()
        {
            StartSearch();
        }

        private void Error(Exception exception)
        {
            ReportError(TextSource.GetText("Error.Twitter") + ": " + exception.Message);
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
			TweetsPlus = Tweets.Select(x => new WithCommand<Tweet>(x, new MvxCommand(() => ShowTweet(x)))).ToList();
			WhenLastUpdatedUtc = DateTime.UtcNow;
        }
			                           
		private void ShowTweet(Tweet tweet)
		{
            var guessTwitterNameEnds = tweet.Author.IndexOf(' ');
            if (guessTwitterNameEnds > 0)
            {
                var guessTwitterName = tweet.Author.Substring(0, guessTwitterNameEnds);
                ExceptionSafeShare("@" + guessTwitterName + " #sqlbitsX ");
            }
            else
            {
                ExceptionSafeShare("@" + tweet.Author + " #sqlbitsX ");
            }
		}
    }
}