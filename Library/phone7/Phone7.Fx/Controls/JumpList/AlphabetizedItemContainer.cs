using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Phone7.Fx.Controls.JumpList
{
    public class AlphabetizedItemContainer : INotifyPropertyChanged
    {
        private char _firstLetter;
        public char FirstLetter
        {
            get { return _firstLetter; }
            set { _firstLetter = value; RaisePropertyChanged("FirstLetter"); }
        }

        private object _item;
        public object Item
        {
            get { return _item; }
            set { _item = value; RaisePropertyChanged("Item"); }
        }

        private bool _showGroup;
        public bool ShowGroup
        {
            get { return _showGroup; }
            set { _showGroup = value; RaisePropertyChanged("ShowGroup"); }
        }



        public bool HasImage
        {
            get { return !string.IsNullOrEmpty(ImageSource); }
        }

        private string _imageSource;
        public string ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; RaisePropertyChanged("ImageSource"); RaisePropertyChanged("HasImage"); }
        }

        //private ObservableCollection<T> _items;
        //public ObservableCollection<T> Items
        //{
        //    get { return _items; }
        //    set { _items = value; RaisePropertyChanged("Items"); }
        //}

        private void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}