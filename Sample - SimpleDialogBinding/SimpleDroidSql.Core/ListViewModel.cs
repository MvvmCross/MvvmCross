using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Cirrious.MvvmCross.ViewModels;

namespace SimpleDroidSql
{
    public class ListViewModel
        : INotifyPropertyChanged
        
    {
        private readonly DatabaseBackedObservableCollection<ListItem, int> _items;
        private string _textToAdd;

        public IList Items { get { return _items; } }

        public string TextToAdd
        {
            get { return _textToAdd; }
            set { _textToAdd = value; RaisePropertyChanged("TextToAdd"); }
        }

        public ICommand AddCommand
        {
            get { return new MvxCommand(DoAdd); }
        }

        private void DoAdd()
        {
            if (string.IsNullOrWhiteSpace(TextToAdd))
            {
                // Todo - signal the error... although really UI should not send this!
                return;
            }

            _items.Add(new ListItem() { Name = TextToAdd, WhenCreated = DateTime.Now.ToString("HH:mm:ss ddd MMM yyyy") });
            TextToAdd = string.Empty;
        }

            public ListViewModel()
            {
                Cirrious.MvvmCross.Plugins.Sqlite.PluginLoader.Instance.EnsureLoaded();
                var factory = Mvx.Resolve<ISQLiteConnectionFactory>();
                var connection = factory.Create("SimpleList");
                connection.CreateTable<ListItem>();
                _items = new DatabaseBackedObservableCollection<ListItem, int>(connection, listItem => -listItem.Id);
            }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}