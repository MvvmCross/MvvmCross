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
            PickRandomSearchTerm();
        }

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

        public ICommand SearchCommand
        {
            get
            {
                return new MvxRelayCommand(DoSearch);
            }
        }

        public ICommand PickRandomCommand
        {
            get
            {
                return new MvxRelayCommand(PickRandomSearchTerm);
            }
        }

        private void DoSearch()
        {
            if (SearchText == "javascript")
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            RequestNavigate<TwitterViewModel>(new { searchTerm = SearchText });
        }

        private void PickRandomSearchTerm()
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