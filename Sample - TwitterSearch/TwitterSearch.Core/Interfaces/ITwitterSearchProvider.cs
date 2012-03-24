using System;
using System.Collections.Generic;
using TwitterSearch.Core.Models;

namespace TwitterSearch.Core.Interfaces
{
    public interface ITwitterSearchProvider
    {
        void StartAsyncSearch(string searchText,
                              Action<IEnumerable<Tweet>> success, 
                              Action<Exception> error);
    }
}