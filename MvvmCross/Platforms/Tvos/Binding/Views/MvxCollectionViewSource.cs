// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Views
{
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
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
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
                var collectionChanged = _itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                {
                    _subscription = collectionChanged.WeakSubscribe(CollectionChangedOnCollectionChanged);
                }
                ReloadData();
            }
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ItemsSource?.ElementAt(indexPath.Row);
        }

        /// <summary>
        /// Wait for all animations to finish
        /// </summary>
        public async Task WaitAnimationsCompletedAsync()
        {
            await CollectionView.PerformBatchUpdatesAsync(() => { });
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ReloadData();
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            if (ItemsSource == null)
                return 0;

            return ItemsSource.Count();
        }
    }
}
