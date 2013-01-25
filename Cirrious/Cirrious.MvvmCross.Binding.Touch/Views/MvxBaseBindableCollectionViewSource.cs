// MvxBaseBindableCollectionViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Touch.Interfaces.Views;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public abstract class MvxBaseBindableCollectionViewSource : UICollectionViewSource
	{
		public static readonly NSString UnknownCellIdentifier = null;

		private readonly NSString _cellIdentifier;
		private readonly UICollectionView _collectionView;

		protected virtual NSString DefaultCellIdentifier
		{
			get { return _cellIdentifier; }
		}
		
		protected MvxBaseBindableCollectionViewSource (UICollectionView collectionView)
			: this(collectionView, UnknownCellIdentifier)
		{
		}

		protected MvxBaseBindableCollectionViewSource(UICollectionView collectionView,
		                                         NSString cellIdentifier)
		{
			_collectionView= collectionView;
			_cellIdentifier = cellIdentifier;
		}

		protected UICollectionView CollectionView
		{
			get { return _collectionView; }
		}

		public event EventHandler<MvxSimpleSelectionChangedEventArgs> SelectionChanged;
		
		public ICommand SelectionChangedCommand { get; set; }
			
		public virtual void ReloadData()
		{
			_collectionView.ReloadData();
		}
		
		protected virtual UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
		{
			return (UICollectionViewCell)collectionView.DequeueReusableCell(DefaultCellIdentifier, indexPath);
		}

		protected abstract object GetItemAt(NSIndexPath indexPath);

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var item = GetItemAt(indexPath);
			var selectionChangedArgs = MvxSimpleSelectionChangedEventArgs.JustAddOneItem(item);
			
			var handler = SelectionChanged;
			if (handler != null)
				handler(this, selectionChangedArgs);
			
			var command = SelectionChangedCommand;
			if (command != null)
				command.Execute(item);
		}
		
		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var item = GetItemAt(indexPath);
			var cell = GetOrCreateCellFor(collectionView, indexPath, item);
			
			var bindable = cell as IMvxBindableView;
			if (bindable != null)
				bindable.BindTo(item);
			
			return cell;
		}
		
		public override int NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}
	}
}