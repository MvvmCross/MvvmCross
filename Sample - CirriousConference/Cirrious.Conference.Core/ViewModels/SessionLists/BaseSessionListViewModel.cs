using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.Conference.Core.Models;
using Cirrious.Conference.Core.Models.Raw;
using Cirrious.Conference.Core.ViewModels.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ViewModels.SessionLists
{
    public class BaseSessionListViewModel<TKey>
        : BaseConferenceViewModel
    {
        public class SessionGroup : List<WithCommand<SessionWithFavoriteFlag>>
        {
            public TKey Key { get; set; }

            public SessionGroup(TKey key, IEnumerable<SessionWithFavoriteFlag> items, Action<Session> tapAction)
                : base((IEnumerable<WithCommand<SessionWithFavoriteFlag>>) items.Select(x => new WithCommand<SessionWithFavoriteFlag>(x, new MvxCommand(() => tapAction(x.Session)))))
            {
                Key = key;
            }
        }
		
        private List<SessionGroup> _groupedList;
        public List<SessionGroup> GroupedList
        {
            get { return _groupedList; }
            protected set { _groupedList = value; RaisePropertyChanged("GroupedList"); }
        }

        protected void NavigateToSession(Session session)
        {
            ShowViewModel<SessionViewModel>(new { key = session.Key });
        }
    }
}