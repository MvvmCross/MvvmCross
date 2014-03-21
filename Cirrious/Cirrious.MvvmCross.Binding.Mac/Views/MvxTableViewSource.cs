// MvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.Collections;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using System.Collections.Specialized;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.CrossCore.Core;
using System.Linq;
using System.Windows.Input;

namespace Cirrious.MvvmCross.Binding.Mac.Views
{
	public class MvxTableViewSource : NSTableViewSource
	{
        IEnumerable _itemsSource;
        IDisposable _subscription;
        NSTableView _tableView;

		public MvxTableViewSource (NSTableView tableView): base()
		{
			this._tableView = tableView;
		}

		public override int GetRowCount (NSTableView tableView)
		{
			return ItemsSource.Count ();
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

				ReloadTableData();
			}
		}

		void ReloadTableData ()
		{
			_tableView.ReloadData ();
		}

		NSView GetOrCreateViewFor (NSTableView tableView, NSTableColumn tableColumn)
		{
			var view = tableView.MakeView (tableColumn.Identifier, this);
			var bindableColumn = tableColumn as MvxTableColumn;
			if (bindableColumn != null){
				if (view == null)
					view = new MvxTableCellView (bindableColumn.BindingText);
				else {
					IMvxBindingContextOwner bindableView = view as IMvxBindingContextOwner;
					bindableView.CreateBindingContext (bindableColumn.BindingText);
				}
			}
			return view;
		}

		public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, int row)
		{
			if (ItemsSource == null)
				return null;

			var item =  ItemsSource.ElementAt(row);
			var view = GetOrCreateViewFor (tableView, tableColumn);

			var bindable = view as IMvxDataConsumer;
			if (bindable != null)
				bindable.DataContext = item;

			return view;
		}

		protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			TryDoAnimatedChange (args);
		}

		protected static NSIndexSet CreateNSIndexSet(int startingPosition, int count)
		{
			return NSIndexSet.FromArray (Enumerable.Range(startingPosition, count).ToArray());
		}

		public ICommand SelectionChangedCommand {
			get;
			set;
		}

        public bool ReloadOnAllItemsSourceSets { get; set; }

		public override void SelectionDidChange (NSNotification notification)
		{
			var command = SelectionChangedCommand;
			if (command == null)
				return;

			var row = this._tableView.SelectedRow;
			if (row < 0)
				return;

			var item = ItemsSource.ElementAt (row);

			if (!command.CanExecute (item))
				return;

			command.Execute(item);
		}

		protected bool TryDoAnimatedChange(NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Add:
			{
				var newIndexSet = CreateNSIndexSet(args.NewStartingIndex, args.NewItems.Count);
				_tableView.InsertRows (newIndexSet, NSTableViewAnimation.Fade);
				return true;
			}
				case NotifyCollectionChangedAction.Remove:
			{
				var newIndexSet = CreateNSIndexSet(args.OldStartingIndex, args.OldItems.Count);
				_tableView.RemoveRows (newIndexSet, NSTableViewAnimation.Fade);
				return true;
			}
				case NotifyCollectionChangedAction.Move:
			{
				if (args.NewItems.Count != 1 && args.OldItems.Count != 1)
					return false;
				_tableView.MoveRow (args.OldStartingIndex, args.NewStartingIndex);
				return true;
			}
				case NotifyCollectionChangedAction.Replace:
			{

				_tableView.ReloadData ();
				return true;
			}
				default:
				return false;
			}
		}
	}
}