// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Foundation;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Logging;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
{
    public abstract class MvxBaseCollectionViewSource : UICollectionViewSource
    {
        public event EventHandler SelectedItemChanged;

        private readonly WeakReference<UICollectionView> _collectionView;
        private object _selectedItem;

        public static readonly NSString UnknownCellIdentifier = NSString.Empty;

        protected virtual NSString DefaultCellIdentifier { get; }

        protected MvxBaseCollectionViewSource(UICollectionView collectionView)
            : this(collectionView, UnknownCellIdentifier)
        {
        }

        protected MvxBaseCollectionViewSource(UICollectionView collectionView,
                                              NSString cellIdentifier)
        {
            _collectionView = new WeakReference<UICollectionView>(collectionView);
            DefaultCellIdentifier = cellIdentifier;
        }

        protected UICollectionView CollectionView
        {
            get
            {
                if (_collectionView.TryGetTarget(out var collectionView))
                    return collectionView;

                // This is not a array Sonar. You are drunk...
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }
        }

        public ICommand SelectionChangedCommand { get; set; }

        public virtual void ReloadData()
        {
            try
            {
                CollectionView.ReloadData();
            }
            catch (Exception exception)
            {
                MvxLogHost.GetLog<MvxBaseCollectionViewSource>()?.Log(LogLevel.Warning, exception,
                    "Exception masked during CollectionView ReloadData");
            }
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);

            if (SelectionChangedCommand?.CanExecute(item) == true)
                SelectionChangedCommand.Execute(item);

            SelectedItem = item;
        }

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                // note that we only expect this to be called from the control/Table
                // we don't have any multi-select or any scroll into view functionality here
                _selectedItem = value;
                SelectedItemChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath, item);

            if (cell is IMvxDataConsumer bindable)
                bindable.DataContext = item;

            return cell;
        }

        public override void CellDisplayingEnded(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
        {
            //Don't bind to NULL to speed up cells in lists when fast scrolling
            //There should be almost no scenario in which this is required
            //If it is required, do this in your own subclass using this code:

            //var bindable = cell as IMvxDataConsumer;
            //if (bindable != null)
            //    bindable.DataContext = null;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return 0;
        }

        protected virtual UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath,
            object item)
        {
            return (UICollectionViewCell)collectionView.DequeueReusableCell(DefaultCellIdentifier, indexPath);
        }

        protected abstract object GetItemAt(NSIndexPath indexPath);
    }
}
