using System;
using System.Collections.Generic;
using Cirrious.Conference.Core.Models.Twitter;

namespace Cirrious.Conference.Core.Interfaces
{
    public interface ITwitterSearchProvider
    {
        void StartAsyncSearch(string searchText, Action<IEnumerable<Tweet>> success, Action<Exception> error);
    }
}