// MvxBaseCollectionViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Foundation;
using System;
using System.Windows.Input;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public abstract class MvxBaseCollectionViewSource : UICollectionViewSource
    {
        public static readonly NSString UnknownCellIdentifier = null;

        private readonly NSString _cellIdentifier;
        private readonly UICollectionView _collectionView;

        protected virtual NSString DefaultCellIdentifier => _cellIdentifier;

        protected MvxBaseCollectionViewSource(UICollectionView collectionView)
            : this(collectionView, UnknownCellIdentifier)
        {
        }

        protected MvxBaseCollectionViewSource(UICollectionView collectionView,
                                              NSString cellIdentifier)
        {
            _collectionView = collectionView;
            _cellIdentifier = cellIdentifier;
        }

        protected UICollectionView CollectionView => _collectionView;

        public ICommand SelectionChangedCommand { get; set; }

        public virtual void ReloadData()
        {
            try
            {
                _collectionView.ReloadData();
            }
            catch (Exception exception)
            {
                Mvx.Warning("Exception masked during CollectionView ReloadData {0}", exception.ToLongString());
            }
        }

        protected virtual UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath,
                                                                  object item)
        {
            return (UICollectionViewCell)collectionView.DequeueReusableCell(DefaultCellIdentifier, indexPath);
        }

        protected abstract object GetItemAt(NSIndexPath indexPath);

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);

            var command = SelectionChangedCommand;
            if (command != null && command.CanExecute(item))
                command.Execute(item);

            SelectedItem = item;
        }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                // note that we only expect this to be called from the control/Table
                // we don't have any multi-select or any scroll into view functionality here
                _selectedItem = value;
                var handler = SelectedItemChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedItemChanged;

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath, item);

            var bindable = cell as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = item;

            return cell;
        }

        public override void CellDisplayingEnded(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
        {
            var bindable = cell as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = null;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }
    }
}