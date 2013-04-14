/**
 * The original AddRange method code took from Nathan Nesbit's Blog
 * http://blogs.msdn.com/b/nathannesbit/archive/2009/04/20/addrange-and-observablecollection.aspx
 * 
 * Few optimizations applied to the AddRange/RemoveRange logic.
 */
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.ViewModels
{
	public class MvxObservableCollection<T>
		: ObservableCollection<T>
		, IList<T>
	{
		public MvxObservableCollection()
		{
		}
		
		public MvxObservableCollection(IEnumerable<T> source)
			: base(source)
		{
		}
		
		public void AddRange(IEnumerable<T> items)
		{
			if (items == null)
				return;
			
			this.CheckReentrancy();
			
			//
			// We need the starting index later
			//
			int startingIndex = this.Count;
			
			bool isList = items is IList<T>;
			IList<T> itemsList = items as IList<T>;
			
			//
			// Add the items directly to the inner collection
			//
			foreach (var item in items)
			{
				this.Items.Add(item);
				if (!isList)
				{
					if (itemsList == null)
						itemsList = new List<T>();
					itemsList.Add(item);
				}
			}
			
			if (startingIndex == this.Count)
				return;
			
			//
			// Now raise the changed events
			//
			this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			
			//
			// We have to change our input of new items into an IList since that is what the
			// event args require.
			//
			var changedItems = new ReadOnlyCollection<T>(itemsList);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startingIndex));
		}
		
		public void RemoveRange(IEnumerable<T> items)
		{
			this.CheckReentrancy();
			
			//
			// We need the starting index later
			//
			int startingIndex = this.Count;
			
			bool isList = items is IList<T>;
			IList<T> itemsList = items as IList<T>;
			
			//
			// Add the items directly to the inner collection
			//
			foreach (var item in items)
			{
				this.Items.Remove(item);
				if (!isList)
				{
					if (itemsList == null)
						itemsList = new List<T>();
					itemsList.Add(item);
				}
			}
			
			if (startingIndex == this.Count)
				return;
			
			//
			// Now raise the changed events
			//
			this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			
			//
			// We have to change our input of new items into an IList since that is what the
			// event args require.
			//
			var changedItems = new ReadOnlyCollection<T>(itemsList);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItems, startingIndex));
		}

		protected void InvokeOnMainThread(Action action)
		{
			var dispatcher = MvxMainThreadDispatcher.Instance;
			if (dispatcher != null)
				dispatcher.RequestMainThreadAction(action);
		}
		
		protected override void OnCollectionChanged (NotifyCollectionChangedEventArgs e)
		{
			InvokeOnMainThread(() => base.OnCollectionChanged(e));
		}
		
		protected override void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			InvokeOnMainThread(() => base.OnPropertyChanged(e));
		}
	}
}
