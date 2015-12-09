// MvxCollectionViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Foundation;
using System;
using System.Collections;
using System.Collections.Specialized;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
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
                ReloadData();
            }
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ItemsSource?.ElementAt(indexPath.Row);
        }

        private void CollectionChangedOnCollectionChanged(object sender,
                                                          NotifyCollectionChangedEventArgs
                                                              notifyCollectionChangedEventArgs)
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