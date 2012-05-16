using System;
using System.Collections.Generic;
using System.ComponentModel;
using DroidAutoComplete.Books;

namespace DroidAutoComplete
{
    public class BooksViewModel
        : INotifyPropertyChanged
    {
        private string _enteredText;
        public string EnteredText
        {
            get { return _enteredText; }
            private set { _enteredText = value; FirePropertyChanged("EnteredText"); }
        }

        private string _currentTextHint;
        public string CurrentTextHint
        {
            get { return _currentTextHint; }
            set
            {
                _currentTextHint = value;
                if (_currentTextHint == null
                    || _currentTextHint.Trim().Length < 5)
                {
                    SetSuggestionsEmpty();
                    return;
                }

                BeginAsyncLookup();
            }
        }

        private List<BooksJsonService.BookSearchItem> _autoCompleteSuggestions;
        public List<BooksJsonService.BookSearchItem> AutoCompleteSuggestions
        {
            get { return _autoCompleteSuggestions; }
            set { _autoCompleteSuggestions = value; FirePropertyChanged("AutoCompleteSuggestions"); }
        }

        private BooksJsonService.BookSearchItem _currentBook;

        public BooksJsonService.BookSearchItem CurrentBook
        {
            get { return _currentBook; }
            set { _currentBook = value; FirePropertyChanged("CurrentBook"); }
        }

        private IMainThreadRunner _mainThreadRunner; 
        public BooksViewModel(IMainThreadRunner mainThreadRunner)
        {
            _mainThreadRunner = mainThreadRunner;
            SetSuggestionsEmpty();
        }

        private void SetSuggestionsEmpty()
        {
            AutoCompleteSuggestions = new List<BooksJsonService.BookSearchItem>();
        }

        #region The search code

        private void BeginAsyncLookup()
        {
            var service = new BooksJsonService();
            service.StartSearchAsync(this.CurrentTextHint, SearchSuccess, SearchFailure);
        }

        private void SearchSuccess(BooksJsonService.BookSearchResult success)
        {
            var autoCompleteSuggestions = new List<BooksJsonService.BookSearchItem>(success.items);
            AutoCompleteSuggestions = autoCompleteSuggestions;
        }

        private void SearchFailure(Exception obj)
        {
            SetSuggestionsEmpty();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string whichProperty)
        {
            // take a copy - see RoadWarrior's answer on http://stackoverflow.com/questions/282653/checking-for-null-before-event-dispatching-thread-safe/282741#282741
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(whichProperty));
        }

        #endregion
    }
}