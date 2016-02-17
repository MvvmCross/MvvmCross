using Foundation;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UIKit;

namespace MvvmCross.iOS.Support.Views
{
    public abstract class MvxExpandableTableSource : MvxTableViewSource
    {
        private bool[] _isCollapsed;

        public MvxExpandableTableSource(UITableView tableView) : base(tableView)
        {
        }

        protected override void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            _isCollapsed = new bool[ItemsSource.Count()];

            for (var i = 0; i < _isCollapsed.Length; i++)
                _isCollapsed[i] = true;

            base.CollectionChangedOnCollectionChanged(sender, args);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (((IEnumerable<object>)ItemsSource?.ElementAt((int)section)).Any() == true && !_isCollapsed[(int)section])
                return ((IEnumerable<object>)ItemsSource.ElementAt((int)section)).Count();
            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return ItemsSource.Count();
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (ItemsSource == null)
                return null;

            return ((IEnumerable<object>)ItemsSource.ElementAt(indexPath.Section)).ElementAt(indexPath.Row);
        }

        protected object GetHeaderItemAt(nint section)
        {
            if (ItemsSource == null)
                return null;

            return ItemsSource.ElementAt((int)section);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var header = GetOrCreateHeaderCellFor(tableView, section);

            UIButton hiddenButton = new UIButton(header.Frame);
            hiddenButton.TouchUpInside += EventHandler(tableView, section);
            header.AddSubview(hiddenButton);

            var bindable = header as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = GetHeaderItemAt(section);
            return header;
        }

        private EventHandler EventHandler(UITableView tableView, nint section)
        {
            return (sender, e) =>
            {
                _isCollapsed[(int)section] = !_isCollapsed[(int)section];
                tableView.ReloadData();
            };
        }

        public override void HeaderViewDisplayingEnded(UITableView tableView, UIView headerView, nint section)
        {
            var bindable = headerView as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = null;
        }

        /// <summary>
        /// This is needed to show the header view. Should be overriden by sources that inherit from this.
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 40; // Default value.
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return base.GetCell(tableView, indexPath);
        }

        protected abstract UITableViewCell GetOrCreateHeaderCellFor(UITableView tableView, nint section);
        protected abstract override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item);
    }
}
