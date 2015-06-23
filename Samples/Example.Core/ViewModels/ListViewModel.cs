using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Example.Core.ViewModels
{
    public class ListViewModel 
		: MvxViewModel
    {
		
        private List<ListItem> _items;

        public List<ListItem> Items {
            get { return _items; }
            set {
                _items = value;
                RaisePropertyChanged (() => Items);
            }
        }

        public virtual ICommand SelectedItem {
            get { 
                return new MvxCommand (null
					/*() => ShowViewModel<AvailableRouteSummaryViewModel> (
						new { name = SelectedRoute.Name, code = SelectedRoute.Code }  )*/
                ); 
            }
        }

        private bool _isRefreshing;

        public virtual bool IsRefreshing {
            get { return _isRefreshing; }
            set {
                _isRefreshing = value;
                RaisePropertyChanged (() => IsRefreshing);
            }
        }

        public ICommand ReloadCommand {
            get {
                return new MvxCommand (async () => {
                    IsRefreshing = true;

                    await ReloadData ();

                    IsRefreshing = false; 
                });
            }
        }

        public virtual async Task ReloadData ()
        { 
            // By default return a completed Task
            await Task.Delay (5000);
        }
    }
}
