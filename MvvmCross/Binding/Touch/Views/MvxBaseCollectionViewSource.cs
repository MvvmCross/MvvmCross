// MvxBaseCollectionViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Views
{
    using System;
    using System.Windows.Input;

    using Foundation;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;

    using UIKit;

    public abstract class MvxBaseCollectionViewSource : UICollectionViewSource
    {
        public static readonly NSString UnknownCellIdentifier = null;

        private readonly NSString _cellIdentifier;
        private readonly UICollectionView _collectionView;

        protected virtual NSString DefaultCellIdentifier => this._cellIdentifier;

        protected MvxBaseCollectionViewSource(UICollectionView collectionView)
            : this(collectionView, UnknownCellIdentifier)
        {
        }

        protected MvxBaseCollectionViewSource(UICollectionView collectionView,
                                              NSString cellIdentifier)
        {
            this._collectionView = collectionView;
            this._cellIdentifier = cellIdentifier;
        }

        protected UICollectionView CollectionView => this._collectionView;

        public ICommand SelectionChangedCommand { get; set; }

        public virtual void ReloadData()
        {
            try
            {
                this._collectionView.ReloadData();
            }
            catch (Exception exception)
            {
                Mvx.Warning("Exception masked during CollectionView ReloadData {0}", exception.ToLongString());
            }
        }

        protected virtual UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath,
                                                                  object item)
        {
            return (UICollectionViewCell)collectionView.DequeueReusableCell(this.DefaultCellIdentifier, indexPath);
        }

        protected abstract object GetItemAt(NSIndexPath indexPath);

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = this.GetItemAt(indexPath);

            var command = this.SelectionChangedCommand;
            if (command != null && command.CanExecute(item))
                command.Execute(item);

            this.SelectedItem = item;
        }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return this._selectedItem; }
            set
            {
                // note that we only expect this to be called from the control/Table
                // we don't have any multi-select or any scroll into view functionality here
                this._selectedItem = value;
                var handler = this.SelectedItemChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedItemChanged;

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = this.GetItemAt(indexPath);
            var cell = this.GetOrCreateCellFor(collectionView, indexPath, item);

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