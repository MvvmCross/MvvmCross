// MvxBaseCollectionViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows.Input;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public abstract class MvxBaseCollectionViewSource : UICollectionViewSource
    {
        public static readonly NSString UnknownCellIdentifier = null;

        private readonly NSString _cellIdentifier;
        private readonly UICollectionView _collectionView;

        protected virtual NSString DefaultCellIdentifier
        {
            get { return _cellIdentifier; }
        }

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

        protected UICollectionView CollectionView
        {
            get { return _collectionView; }
        }

        public ICommand SelectionChangedCommand { get; set; }

        public virtual void ReloadData()
        {
            _collectionView.ReloadData();
        }

        protected virtual UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath,
                                                                  object item)
        {
            return (UICollectionViewCell) collectionView.DequeueReusableCell(DefaultCellIdentifier, indexPath);
        }

        protected abstract object GetItemAt(NSIndexPath indexPath);

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);

            var command = SelectionChangedCommand;
            if (command != null)
                command.Execute(item);
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath, item);

            var bindable = cell as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = item;

            return cell;
        }

        public override int NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }
    }
}