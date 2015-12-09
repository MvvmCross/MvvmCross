// MvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace Cirrious.MvvmCross.Binding.Mac.Views
{
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
            return ItemsSource.Count();
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (Object.ReferenceEquals(_itemsSource, value)
                    && !ReloadOnAllItemsSourceSets)
                    return;

                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }

                _itemsSource = value;

                var collectionChanged = _itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                {
                    _subscription = collectionChanged.WeakSubscribe(CollectionChangedOnCollectionChanged);
                }

                ReloadTableData();
            }
        }

        private void ReloadTableData()
        {
            _tableView.ReloadData();
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
            if (ItemsSource == null)
                return null;

            var item = ItemsSource.ElementAt((int)row);
            var view = GetOrCreateViewFor(tableView, tableColumn);

            var bindable = view as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = item;

            return view;
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            TryDoAnimatedChange(args);
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
            var command = SelectionChangedCommand;
            if (command == null)
                return;

            var row = this._tableView.SelectedRow;
            if (row < 0)
                return;

            var item = ItemsSource.ElementAt((int)row);

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
                        _tableView.InsertRows(newIndexSet, NSTableViewAnimation.Fade);
                        return true;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        var newIndexSet = CreateNSIndexSet(args.OldStartingIndex, args.OldItems.Count);
                        _tableView.RemoveRows(newIndexSet, NSTableViewAnimation.Fade);
                        return true;
                    }
                case NotifyCollectionChangedAction.Move:
                    {
                        if (args.NewItems.Count != 1 && args.OldItems.Count != 1)
                            return false;
                        _tableView.MoveRow(args.OldStartingIndex, args.NewStartingIndex);
                        return true;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        _tableView.ReloadData();
                        return true;
                    }
                default:
                    return false;
            }
        }
    }
}