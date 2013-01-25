// MvxBindableCollectionViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public abstract class MvxBindableCollectionViewSource : MvxBaseBindableCollectionViewSource
	{
		private IEnumerable _itemsSource;
		
		public MvxBindableCollectionViewSource(UICollectionView collectionView)
			: base(collectionView)
		{
		}
		
		public MvxBindableCollectionViewSource(UICollectionView collectionView,
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
				if (_itemsSource == value)
					return;
				
				var collectionChanged = _itemsSource as INotifyCollectionChanged;
				if (collectionChanged != null)
					collectionChanged.CollectionChanged -= CollectionChangedOnCollectionChanged;
				_itemsSource = value;
				collectionChanged = _itemsSource as INotifyCollectionChanged;
				if (collectionChanged != null)
					collectionChanged.CollectionChanged += CollectionChangedOnCollectionChanged;
				ReloadData();
			}
		}
		
		protected override object GetItemAt(NSIndexPath indexPath)
		{
			if (ItemsSource == null)
				return null;
			
			return ItemsSource.ElementAt(indexPath.Row);
		}
		
		protected virtual void CollectionChangedOnCollectionChanged(object sender,
		                                                            NotifyCollectionChangedEventArgs
		                                                            notifyCollectionChangedEventArgs)
		{
			ReloadData();
		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			if (ItemsSource == null)
				return 0;
			
			return ItemsSource.Count();
		}
	}
}