﻿// MvxView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using AppKit;
using Foundation;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Mac.Views
{
    public class MvxTableViewSource : NSTableViewSource
    {
        private IEnumerable _itemsSource;
        private IDisposable _subscription;
        private NSTableView _tableView;

        public MvxTableViewSource(NSTableView tableView) : base()
        {
            _tableView = tableView;
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return ItemsSource.Count();
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get
            {
                return _itemsSource;
            }
            set
            {
                if (ReferenceEquals(_itemsSource, value)
                    && !ReloadOnAllItemsSourceSets)
                    return;

                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }

                _itemsSource = value;

                if (_itemsSource is INotifyCollectionChanged collectionChanged)
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

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            if (ItemsSource == null)
                return null;

            var item = ItemsSource.ElementAt((int)row);
            var view = GetOrCreateViewFor(tableView, tableColumn);

            if (view is IMvxDataConsumer bindable)
                bindable.DataContext = item;

            return view;
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Action action = () =>
            {
                if (!UseAnimations)
                {
                    ReloadTableData();
                }
                else
                {
                    if (TryDoAnimatedChange(args))
                        return;

                    ReloadTableData();
                }
            };

            if (NSThread.IsMain)
                action();
            else
                InvokeOnMainThread(action);
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
        public bool UseAnimations { get; set; }

        public override void SelectionDidChange(NSNotification notification)
        {
            var command = SelectionChangedCommand;
            if (command == null)
                return;

            var row = _tableView.SelectedRow;
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