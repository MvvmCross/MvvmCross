using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Example.Core.ViewModels
{
    public class RecyclerViewModel
        : MvxViewModel
    {
        public RecyclerViewModel()
        {
            this.Items = new List<ListItem>() {
                new ListItem () { Title = "title one" },
                new ListItem () { Title = "title two" },
                new ListItem () { Title = "title three" },
                new ListItem () { Title = "title four" },
                new ListItem () { Title = "title five" },
            };
        }

        private List<ListItem> _items;

        public List<ListItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged(() => Items);
            }
        }

        public virtual ICommand SelectedItem
        {
            get
            {
                return new MvxCommand(null);
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
        }
    }
}