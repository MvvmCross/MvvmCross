// MvxCollectionViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Threading.Tasks;

    using Foundation;

    using MvvmCross.Binding.Attributes;
    using MvvmCross.Binding.ExtensionMethods;
    using MvvmCross.Platform.WeakSubscription;

    using UIKit;

    public class MvxCollectionViewSource : MvxBaseCollectionViewSource
    {
        private IEnumerable _itemsSource;
        private IDisposable _subscription;

        public bool ReloadOnAllItemsSourceSets { get; set; }

        public MvxCollectionViewSource(UICollectionView collectionView)
            : base(collectionView)
        {
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

        public MvxCollectionViewSource(UICollectionView collectionView,
                                       NSString defaultCellIdentifier)
            : base(collectionView, defaultCellIdentifier)
        {
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
                this.ReloadData();
            }
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return this.ItemsSource?.ElementAt(indexPath.Row);
        }
            
        /// <summary>
        /// Wait for all animations to finish
        /// </summary>
        public async Task WaitAnimationsCompleted()
        {
            await CollectionView.PerformBatchUpdatesAsync(() => { }); 
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                CollectionView.PerformBatchUpdates(() => 
                {
                    var startIndex = args.OldStartingIndex;
                    var indexes = new NSIndexPath[args.OldItems.Count];
                    for(var i=0; i<indexes.Length; i++)
                        indexes[i] = NSIndexPath.FromRowSection(startIndex+i, 0);
                    CollectionView.DeleteItems(indexes);
                }, ok => { });
            }
            else if (args.Action == NotifyCollectionChangedAction.Add)
            {
                CollectionView.PerformBatchUpdates(() =>
                {
                    var startIndex = args.NewStartingIndex;
                    var indexes = new NSIndexPath[args.NewItems.Count];
                    for (var i = 0; i < indexes.Length; i++)
                        indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);
                    CollectionView.InsertItems(indexes);
                }, ok => { });
            }
            else if (args.Action == NotifyCollectionChangedAction.Move)
            {
                CollectionView.PerformBatchUpdates(() =>
                {
                    var oldCount = args.OldItems.Count;
                    var newCount = args.NewItems.Count;
                    var indexes = new NSIndexPath[oldCount + newCount];

                    var startIndex = args.OldStartingIndex;
                    for (var i = 0; i < oldCount; i++)
                        indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);
                    startIndex = args.NewStartingIndex;
                    for (var i = oldCount; i < oldCount + newCount; i++)
                        indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);

                    CollectionView.ReloadItems(indexes);
                }, ok => { });
            }
            else
            {
                //Wait for all previous updates / animations to finish, otherwise it crashes with a consistency exception.
                //Unit test case: using a non empty ObservableCollection, call .Clear() then .Add(items) on the observablecollection.
                CollectionView.PerformBatchUpdates(() => { }, animationsCompleted =>
                {
                    ReloadData();
                });
            }
        }
        
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            if (this.ItemsSource == null)
                return 0;

            return this.ItemsSource.Count();
        }
    }
}
