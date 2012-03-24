using System;
using System.Collections.Generic;
using Cirrious.Conference.Core.Interfaces;

namespace Cirrious.Conference.Core.Models.Twitter
{
    public class TwitterSearchProvider : ITwitterSearchProvider
    {
        public void StartAsyncSearch(string searchText, Action<IEnumerable<Tweet>> success, Action<Exception> error)
        {
            TwitterSearch.StartAsyncSearch(searchText, success, error);
        }        
    }
}