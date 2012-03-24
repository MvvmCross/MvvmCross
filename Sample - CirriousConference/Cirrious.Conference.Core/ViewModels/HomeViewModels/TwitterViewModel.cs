using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.Conference.Core.Models.Twitter;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.Conference.Core.ViewModels.Helpers;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;

namespace Cirrious.Conference.Core.ViewModels.HomeViewModels
{
    public class TwitterViewModel
        : BaseViewModel
        , IMvxServiceConsumer<ITwitterSearchProvider>
		, IMvxServiceConsumer<IMvxReachability>
    {
        private const string SearchTerm = "SQLBits";

        private ITwitterSearchProvider TwitterSearchProvider
        {
            get { return this.GetService<ITwitterSearchProvider>(); }
        }

        private IEnumerable<Tweet> _tweets;
        public IEnumerable<Tweet> Tweets
        {
            get { return _tweets; }
            set { _tweets = value; FirePropertyChanged("Tweets"); }
        }

        private IEnumerable<WithCommand<Tweet>> _tweetsPlus;
        public IEnumerable<WithCommand<Tweet>> TweetsPlus
        {
            get { return _tweetsPlus; }
            set { _tweetsPlus = value; FirePropertyChanged("TweetsPlus"); }
        }
		
        private bool _isSearching;
        public bool IsSearching
        {
            get { return _isSearching; }
            set { _isSearching = value; FirePropertyChanged("IsSearching"); }
        }

        public IMvxCommand SearchCommand
        {
            get
            {
                return new MvxRelayCommand(StartSearch);
            }
        }
		
		public IMvxCommand RefreshCommand
		{
			get
			{
                return new MvxRelayCommand(StartSearch);
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
				FirePropertyChanged("WhenLastUpdatedUtc");
			}
		}
		
        private void StartSearch()
        {
            if (IsSearching)
                return;
			
			IMvxReachability reach;
			if (this.TryGetService<IMvxReachability>(out reach))
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
			TweetsPlus = Tweets.Select(x => new WithCommand<Tweet>(x, new MvxRelayCommand(() => ShowTweet(x)))).ToList();
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