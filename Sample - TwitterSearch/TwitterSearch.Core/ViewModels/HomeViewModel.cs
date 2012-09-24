using System;
using System.Windows.Input;
using Cirrious.MvvmCross.Commands;
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