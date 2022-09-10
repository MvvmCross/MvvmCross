// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Foundation;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.Logging;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
{
    public abstract class MvxTableViewSource : MvxBaseTableViewSource
    {
        private IEnumerable _itemsSource;
        private IDisposable _subscription;

        protected MvxTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        protected MvxTableViewSource(IntPtr handle)
            : base(handle)
        {
            MvxLogHost.Default?.LogWarning("TableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (ReferenceEquals(_itemsSource, value)
                    && !ReloadOnAllItemsSourceSets)
                {
                    return;
                }

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

        public bool ReloadOnAllItemsSourceSets { get; set; }
        public bool UseAnimations { get; set; }
        public UITableViewRowAnimation AddAnimation { get; set; }
        public UITableViewRowAnimation RemoveAnimation { get; set; }
        public UITableViewRowAnimation ReplaceAnimation { get; set; }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ItemsSource?.Count() ?? 0;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ItemsSource?.ElementAt(indexPath.Row);
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender,
                                                                    NotifyCollectionChangedEventArgs args)
        {
            void Action()
            {
                if (!UseAnimations)
                {
                    ReloadTableData();
                }
                else
                {
                    if (TryDoAnimatedChange(args)) return;

                    ReloadTableData();
                }
            }

            if (NSThread.IsMain)
                Action();
            else
                InvokeOnMainThread(Action);
        }

        protected bool TryDoAnimatedChange(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
                return false;

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var newIndexPaths = CreateNSIndexPathArray(args.NewStartingIndex, args.NewItems.Count);
                        TableView.InsertRows(newIndexPaths, AddAnimation);
                        return true;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        var oldIndexPaths = CreateNSIndexPathArray(args.OldStartingIndex, args.OldItems.Count);
                        TableView.DeleteRows(oldIndexPaths, RemoveAnimation);
                        return true;
                    }
                case NotifyCollectionChangedAction.Move:
                    {
                        if (args.NewItems.Count != 1 && args.OldItems.Count != 1)
                            return false;

                        var oldIndexPath = NSIndexPath.FromRowSection(args.OldStartingIndex, 0);
                        var newIndexPath = NSIndexPath.FromRowSection(args.NewStartingIndex, 0);
                        TableView.MoveRow(oldIndexPath, newIndexPath);
                        return true;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        if (args.NewItems.Count != args.OldItems.Count)
                            return false;

                        var indexPaths = Enumerable.Range(args.NewStartingIndex, args.NewItems.Count)
                            .Select(index => NSIndexPath.FromRowSection(index, 0)).ToArray();
                        TableView.ReloadRows(indexPaths, ReplaceAnimation);
                        return true;
                    }
                default:
                    return false;
            }
        }

        protected static NSIndexPath[] CreateNSIndexPathArray(int startingPosition, int count)
        {
            var newIndexPaths = new NSIndexPath[count];
            for (var i = 0; i < count; i++)
            {
                newIndexPaths[i] = NSIndexPath.FromRowSection(i + startingPosition, 0);
            }
            return newIndexPaths;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
            }

            base.Dispose(disposing);
        }
    }
}
