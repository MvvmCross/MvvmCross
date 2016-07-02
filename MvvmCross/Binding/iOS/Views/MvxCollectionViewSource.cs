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
            ReloadData();
        }
        
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            if (this.ItemsSource == null)
                return 0;

            return this.ItemsSource.Count();
        }
    }
}
