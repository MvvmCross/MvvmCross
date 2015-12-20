// MvxView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace MvvmCross.Binding.Mac.Views
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Input;

    using global::MvvmCross.Platform.Core;
    using global::MvvmCross.Platform.WeakSubscription;

    public class MvxTableViewSource : NSTableViewSource
    {
        private IEnumerable _itemsSource;
        private IDisposable _subscription;
        private NSTableView _tableView;

        public MvxTableViewSource(NSTableView tableView) : base()
        {
            this._tableView = tableView;
        }

#if __UNIFIED__
		public override nint GetRowCount (NSTableView tableView)
#else

        public override int GetRowCount(NSTableView tableView)
#endif
        {
            return this.ItemsSource.Count();
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return this._itemsSource; }
            set
            {
                if (Object.ReferenceEquals(this._itemsSource, value)
                    && !this.ReloadOnAllItemsSourceSets)
                    return;

                if (this._subscription != null)
                {
                    this._subscription.Dispose();
                    this._subscription = null;
                }

                this._itemsSource = value;

                var collectionChanged = this._itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                {
                    this._subscription = collectionChanged.WeakSubscribe(this.CollectionChangedOnCollectionChanged);
                }

                this.ReloadTableData();
            }
        }

        private void ReloadTableData()
        {
            this._tableView.ReloadData();
        }

        private NSView GetOrCreateViewFor(NSTableView tableView, NSTableColumn tableColumn)
        {
            var view = tableView.MakeView(tableColumn.Identifier, this);
            var bindableColumn = tableColumn as MvxTableColumn;
            if (bindableColumn != null)
            {
                if (view == null)
                    view = new MvxTableCellView(bindableColumn.BindingText);
                else
                {
                    IMvxBindingContextOwner bindableView = view as IMvxBindingContextOwner;
                    bindableView.CreateBindingContext(bindableColumn.BindingText);
                }
            }
            return view;
        }

#if __UNIFIED__
		public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
#else

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, int row)
#endif
        {
            if (this.ItemsSource == null)
                return null;

            var item = this.ItemsSource.ElementAt((int)row);
            var view = this.GetOrCreateViewFor(tableView, tableColumn);

            var bindable = view as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = item;

            return view;
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.TryDoAnimatedChange(args);
        }

        protected static NSIndexSet CreateNSIndexSet(int startingPosition, int count)
        {
            return NSIndexSet.FromArray(Enumerable.Range(startingPosition, count).ToArray());
        }

        public ICommand SelectionChangedCommand
        {
            get;
            set;
        }

        public bool ReloadOnAllItemsSourceSets { get; set; }

        public override void SelectionDidChange(NSNotification notification)
        {
            var command = this.SelectionChangedCommand;
            if (command == null)
                return;

            var row = this._tableView.SelectedRow;
            if (row < 0)
                return;

            var item = this.ItemsSource.ElementAt((int)row);

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }

        protected bool TryDoAnimatedChange(NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var newIndexSet = CreateNSIndexSet(args.NewStartingIndex, args.NewItems.Count);
                        this._tableView.InsertRows(newIndexSet, NSTableViewAnimation.Fade);
                        return true;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        var newIndexSet = CreateNSIndexSet(args.OldStartingIndex, args.OldItems.Count);
                        this._tableView.RemoveRows(newIndexSet, NSTableViewAnimation.Fade);
                        return true;
                    }
                case NotifyCollectionChangedAction.Move:
                    {
                        if (args.NewItems.Count != 1 && args.OldItems.Count != 1)
                            return false;
                        this._tableView.MoveRow(args.OldStartingIndex, args.NewStartingIndex);
                        return true;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        this._tableView.ReloadData();
                        return true;
                    }
                default:
                    return false;
            }
        }
    }
}