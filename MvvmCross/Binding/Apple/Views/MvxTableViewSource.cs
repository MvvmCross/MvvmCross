// MvxTableViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;

    using Foundation;

    using MvvmCross.Binding.Attributes;
    using MvvmCross.Binding.ExtensionMethods;
    using MvvmCross.Platform;
    using MvvmCross.Platform.WeakSubscription;

    using UIKit;

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
            Mvx.Warning("TableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._subscription != null)
                {
                    this._subscription.Dispose();
                    this._subscription = null;
                }
            }

            base.Dispose(disposing);
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
                    this._subscription = collectionChanged.WeakSubscribe(CollectionChangedOnCollectionChanged);
                }

                this.ReloadTableData();
            }
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
#warning SHARED-APPLE: Missing member 'Row' on NSIndexPath
#warning Fix missing .Row member of NSIndexPath (http://developer.xamarin.com/api/property/Foundation.NSIndexPath.Row/)
#warning Can it be invoked by calling some native method?

            //return this.ItemsSource?.ElementAt(indexPath.Row);

            return this.ItemsSource?.ElementAt(0);
        }

        public bool ReloadOnAllItemsSourceSets { get; set; }
        public bool UseAnimations { get; set; }
        public UITableViewRowAnimation AddAnimation { get; set; }
        public UITableViewRowAnimation RemoveAnimation { get; set; }
        public UITableViewRowAnimation ReplaceAnimation { get; set; }

        protected virtual void CollectionChangedOnCollectionChanged(object sender,
                                                                    NotifyCollectionChangedEventArgs args)
        {
            if (!this.UseAnimations)
            {
                this.ReloadTableData();
                return;
            }

            if (this.TryDoAnimatedChange(args))
            {
                return;
            }

            this.ReloadTableData();
        }

        protected bool TryDoAnimatedChange(NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var newIndexPaths = CreateNSIndexPathArray(args.NewStartingIndex, args.NewItems.Count);
                        this.TableView.InsertRows(newIndexPaths, this.AddAnimation);
                        return true;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        var oldIndexPaths = CreateNSIndexPathArray(args.OldStartingIndex, args.OldItems.Count);
                        this.TableView.DeleteRows(oldIndexPaths, this.RemoveAnimation);
                        return true;
                    }
                case NotifyCollectionChangedAction.Move:
                    {
                        if (args.NewItems.Count != 1 && args.OldItems.Count != 1)
                            return false;

#warning SHARED-APPLE: Missing static member 'FromRowSection' on NSIndexPath

                        //var oldIndexPath = NSIndexPath.FromRowSection(args.OldStartingIndex, 0);
                        //var newIndexPath = NSIndexPath.FromRowSection(args.NewStartingIndex, 0);

                        var oldIndexPath = NSIndexPath.FromIndex((nuint)args.OldStartingIndex);
                        var newIndexPath = NSIndexPath.FromIndex((nuint)args.NewStartingIndex);

                        this.TableView.MoveRow(oldIndexPath, newIndexPath);
                        return true;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        if (args.NewItems.Count != args.OldItems.Count)
                            return false;

#warning SHARED-APPLE: Missing static member 'FromRowSection' on NSIndexPath

                        //var indexPath = NSIndexPath.FromRowSection(args.NewStartingIndex, 0);
                        //this.TableView.ReloadRows(new[]
                        //    {
                        //        indexPath
                        //    }, UITableViewRowAnimation.Fade);

                        var indexPath = NSIndexPath.FromIndex((nuint)args.NewStartingIndex);
                        this.TableView.ReloadRows(new[]
                            {
                                indexPath
                            }, UITableViewRowAnimation.Fade);

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
                //newIndexPaths[i] = NSIndexPath.FromRowSection(i + startingPosition, 0);

#warning SHARED-APPLE: Missing static member 'FromRowSection' on NSIndexPath

                newIndexPaths[i] = NSIndexPath.FromIndex((nuint)i + (nuint)startingPosition);
            }
            return newIndexPaths;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (this.ItemsSource == null)
                return 0;

            return this.ItemsSource.Count();
        }
    }
}