using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Example.Core.Model;

namespace Example.Core.ViewModels
{
    public class ExampleRecyclerViewModel
        : MvxViewModel
    {
        private ListItem _selectedItem;

        public ExampleRecyclerViewModel()
        {
            Items = new ObservableCollection<ListItem> {
                new ListItem { Title = "title one", Name = "1"},
                new ListItem { Title = "title one 2", Name = "1"},
                new ListItem { Title = "title one 3", Name = "1"},
                new ListItem { Title = "title one 4", Name = "1"},
                new ListItem { Title = "title one 5", Name = "1"},

                new ListItem { Title = "title two", Name = "2"},
                new ListItem { Title = "title two 2", Name = "2"},
                new ListItem { Title = "title two 3", Name = "2"},
                new ListItem { Title = "title two 4", Name = "2"},
                new ListItem { Title = "title two 5", Name = "2"},

                new ListItem { Title = "title three", Name = "3" },
                new ListItem { Title = "title three 2", Name = "3" },
                new ListItem { Title = "title three 3", Name = "3" },
                new ListItem { Title = "title three 4", Name = "3" },
                new ListItem { Title = "title three 5", Name = "3" },

                new ListItem { Title = "title four ", Name = "4"},
                new ListItem { Title = "title four 1", Name = "4"},
                new ListItem { Title = "title four 2", Name = "4"},
                new ListItem { Title = "title four 3", Name = "4"},
                new ListItem { Title = "title four 4", Name = "4"},

                new ListItem { Title = "title five", Name = "MvvmCross Awesome ObservableCollectionGrouping"},
                new ListItem { Title = "title five 2", Name = "MvvmCross Awesome ObservableCollectionGrouping"},
                new ListItem { Title = "title five 3", Name = "MvvmCross Awesome ObservableCollectionGrouping"}
            };
        }

        private ObservableCollection<ListItem> _items;

        public ObservableCollection<ListItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged(() => Items);
                RaisePropertyChanged(() => GroupedItems);
            }
        }

        public IEnumerable<IGrouping<string, ListItem>> GroupedItems
        {
            get
            {
                var groupedItems = Items.GroupBy(x => x.Name, x => x);
                return groupedItems;
            }
        }

        public ListItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        public virtual ICommand ItemSelected
        {
            get
            {
                return new MvxCommand<ListItem>(item =>
                {
                    SelectedItem = item;
                });
            }
        }

        private bool _isRefreshing;

        public virtual bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged(() => IsRefreshing);
            }
        }

        public ICommand ReloadCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    IsRefreshing = true;

                    await ReloadData();

                    IsRefreshing = false;
                });
            }
        }

        public virtual async Task ReloadData()
        {
            // By default return a completed Task
            await Task.Delay(5000);

            var rand = new Random();
            Func<char> randChar = () => (char)rand.Next(65, 90);
            Func<int, string> randStr = null;
            randStr = x => (x > 0) ? randStr(--x) + randChar() : "";

            var newItemCount = rand.Next(3);

            for (var i = 0; i < newItemCount; i++)
                Items.Add(new ListItem { Title = "title " + randStr(4) });
        }
    }
}