using System;
using System.Collections.Generic;
using TwitterSearch.Core.Interfaces;

namespace TwitterSearch.Core.Models
{
    public class TwitterSearchProvider : ITwitterSearchProvider
    {
        public void StartAsyncSearch(string searchText, Action<IEnumerable<Tweet>> success, Action<Exception> error)
        {
            TwitterSearch.StartAsyncSearch(searchText, success, error);
        }        
    }
}