using System.Collections;
using System.Collections.Specialized;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public abstract class MvxBindableTableViewDataSource : UITableViewDataSource
    {
        private readonly UITableView _tableView;

        protected MvxBindableTableViewDataSource(UITableView tableView)
        {
            _tableView = tableView;
        }

        private IList _itemsSource;
        public IList ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (_itemsSource == value)
                    return;

                var collectionChanged = _itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                    collectionChanged.CollectionChanged -= CollectionChangedOnCollectionChanged;
                _itemsSource = value;
                collectionChanged = _itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                    collectionChanged.CollectionChanged += CollectionChangedOnCollectionChanged;
                _tableView.ReloadData();
            }
        }

        private void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            _tableView.ReloadData();
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            if (ItemsSource == null)
                return 0;

            return ItemsSource.Count;
        }

        protected abstract UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item);

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (ItemsSource == null)
                return null;

            var item = ItemsSource[indexPath.Row];
            var cell = GetOrCreateCellFor(tableView, indexPath, item);

            var bindable = cell as IMvxBindableView;
            if (bindable != null)
                bindable.BindTo(item);

            return cell;
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}