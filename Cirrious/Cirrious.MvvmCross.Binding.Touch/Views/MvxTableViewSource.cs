// MvxTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Specialized;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public abstract class MvxTableViewSource : MvxBaseTableViewSource
    {
        private IEnumerable _itemsSource;
        private IDisposable _subscription;

        protected MvxTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }

            base.Dispose(disposing);
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (_itemsSource == value)
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

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (ItemsSource == null)
                return null;

            return ItemsSource.ElementAt(indexPath.Row);
        }

        public bool UseAnimations { get; set; }
        public UITableViewRowAnimation AddAnimation { get; set; }
        public UITableViewRowAnimation RemoveAnimation { get; set; }
        public UITableViewRowAnimation ReplaceAnimation { get; set; }

        protected virtual void CollectionChangedOnCollectionChanged(object sender,
                                                                    NotifyCollectionChangedEventArgs args)
        {
            if (!UseAnimations)
            {
                ReloadTableData();
                return;
            }

            if (TryDoAnimatedChange(args))
            {
                return;
            }

            ReloadTableData();
        }

        protected bool TryDoAnimatedChange(NotifyCollectionChangedEventArgs args)
        {
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

                        var indexPath = NSIndexPath.FromItemSection(args.NewStartingIndex, 0);
                        TableView.ReloadRows(new[]
                            {
                                indexPath
                            }, UITableViewRowAnimation.Fade);
                        return true;
                        ;
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

        public override int RowsInSection(UITableView tableview, int section)
        {
            if (ItemsSource == null)
                return 0;

            return ItemsSource.Count();
        }
    }
}