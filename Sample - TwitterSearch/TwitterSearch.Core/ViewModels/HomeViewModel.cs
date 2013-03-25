using System;
using System.Windows.Input;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace TwitterSearch.Core.ViewModels
{
    public class HomeViewModel
        : MvxViewModel
    {
        public void Init()
        {
            Commands = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(this);
            PickRandomCommand();
        }

        public IMvxCommandCollection Commands { get; private set; }

        public class ViewModelState
        {
            public string SearchText { get; set; }
        }

        public void ReloadState(ViewModelState searchText)
        {
            MvxTrace.Trace("ReloadState called with {0}", searchText.SearchText);
            SearchText = searchText.SearchText;
        }

        public ViewModelState SaveState()
        {
            MvxTrace.Trace("SaveState called");
            return new ViewModelState { SearchText = SearchText };
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChanged("SearchText"); }
        }

        public void SearchCommand()
        {
            if (SearchText == "javascript")
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            ShowViewModel<TwitterViewModel>(new { searchTerm = SearchText });
        }

        public void PickRandomCommand()
        {
            var items = new[] { "MvvmCross", "WP7", "MonoTouch", "MonoDroid", "mvvm", "kittens" };
            var r = new Random();
            var originalText = SearchText;
            var newText = originalText;
            while (originalText == newText)
            {
                var which = r.Next(items.Length);
                newText = items[which];
            }
            SearchText = newText;
        }
    }
}