using System;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.ViewModels;

namespace TwitterSearch.Core.ViewModels
{
    public class HomeViewModel
        : MvxViewModel
    {
        public HomeViewModel()
        {
            PickRandomSearchTerm();
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; FirePropertyChanged("SearchText"); }
        }

        public IMvxCommand SearchCommand
        {
            get
            {
                return new MvxRelayCommand(DoSearch);
            }
        }

        public IMvxCommand PickRandomCommand
        {
            get
            {
                return new MvxRelayCommand(PickRandomSearchTerm);
            }
        }

        private void DoSearch()
        {
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