using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Dialog.Touch.AutoView.Interfaces.Lists;
using Foobar.Dialog.Core.Lists;
using Cirrious.MvvmCross.Exceptions;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Views.Lists
{
    public class GeneralListLayout : IListLayout
    {
        private MvxBindableTableViewSource _source;
        private IEnumerable _itemsSource;
        private ICommand _itemClick;
        private IListItemLayout _defaultLayout;
        private Dictionary<string, IListItemLayout> _itemLayouts;

        public GeneralListLayout()
        {
            ItemLayouts = new Dictionary<string, IListItemLayout>();
        }

        public virtual MvxBindableTableViewSource InitialiseSource(UITableView tableView)
        {
            if (_source != null)
            {
                throw new MvxException("You cannot create the source more than once");
            }

            _source = CreateSource(tableView);
            _source.ItemsSource = this.ItemsSource;
            _source.SelectionChangedCommand = this.ItemClick;
            return _source;
        }

        protected virtual MvxBindableTableViewSource CreateSource(UITableView tableView)
        {
            return new GeneralTableViewSource(
                            tableView,
                            DefaultLayout as IMvxLayoutListItemViewFactory, 
                            ItemLayouts.ToDictionary(x => x.Key, x => x.Value as IMvxLayoutListItemViewFactory));
        }

        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { _itemsSource = value; if (_source != null) _source.ItemsSource = _itemsSource; }
        }

        public ICommand ItemClick
        {
            get { return _itemClick; }
            set { _itemClick = value; if (_source != null) _source.SelectionChangedCommand = _itemClick; }
        }

        public IListItemLayout DefaultLayout
        {
            get { return _defaultLayout; }
            set
            {
                CheckListIsNull();
                _defaultLayout = value;
            }
        }

        public Dictionary<string, IListItemLayout> ItemLayouts
        {
            get { return _itemLayouts; }
            private set
            {
                CheckListIsNull();
                _itemLayouts = value;
            }
        }

        private void CheckListIsNull()
        {
            if (_source != null)
            {
                throw new MvxException("You cannot set the default layout after the list has been created.");
            }
        }
    }
}
