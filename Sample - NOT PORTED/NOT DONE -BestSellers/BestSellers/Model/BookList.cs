using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BestSellers
{
    public class BookList : List<Book>
        // ObservableCollection<Book> 
    {
        /*
        public string Category
        {
            get { return _category; }
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Category"));
                }
            }
        }
        string _category;
        */
        public string Category { get; set; }
        public string CategoryDisplayName { get; set; }
    }    
}
