using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class ExampleRecyclerViewModel
        : MvxViewModel
    {
        private ListItem _selectedItem;

        public ExampleRecyclerViewModel()
        {
            Items = new ObservableCollection<ListItem>();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            Task.Run(() =>
            {
                Items.Add(new ListItem { Title = "title one" });
                Items.Add(new ListItem { Title = "title two" });
                Items.Add(new ListItem { Title = "title three" });
                Items.Add(new ListItem { Title = "title four" });
                Items.Add(new ListItem { Title = "title five" });
            });
        }

        private ObservableCollection<ListItem> _items;
        public ObservableCollection<ListItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                SetProperty(ref _items, value);
            }
        }

        public ListItem SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
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
            set { SetProperty(ref _isRefreshing, value); }
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
            randStr = x => x > 0 ? randStr(--x) + randChar() : "";

            var newItemCount = rand.Next(3);

            for (var i = 0; i < newItemCount; i++)
                Items.Add(new ListItem { Title = "title " + randStr(4) });
        }
    }
}