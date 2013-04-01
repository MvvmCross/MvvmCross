using System;
using CrossUI.Touch.Dialog.Elements;
using System.Collections.Generic;
using MonoTouch.UIKit;
using System.Collections.Specialized;
using System.Linq;

namespace Tutorial.UI.Touch.Dialog
{
	public delegate Element ElementBuilderDelegate<T>(T item);
	
	public class SectionEx<T>
		: Section
	{
		#region constructors
		public SectionEx (IList<T> itemsSource, ElementBuilderDelegate<T> builder) : base () 
		{
			ElementBuilder = builder;
			ItemsSource = itemsSource;
		}
		
		/// <summary>
		///  Constructs a Section with the specified header
		/// </summary>
		/// <param name="caption">
		/// The header to display
		/// </param>
		public SectionEx (string caption, IList<T> itemsSource, ElementBuilderDelegate<T> builder) : base (caption)
		{
			ElementBuilder = builder;
			ItemsSource = itemsSource;
		}
		
		/// <summary>
		/// Constructs a Section with a header and a footer
		/// </summary>
		/// <param name="caption">
		/// The caption to display (or null to not display a caption)
		/// </param>
		/// <param name="footer">
		/// The footer to display.
		/// </param>
		public SectionEx (string caption, string footer, IList<T> itemsSource, ElementBuilderDelegate<T> builder) : base (caption, footer)
		{
			ElementBuilder = builder;
			ItemsSource = itemsSource;
		}
		
		public SectionEx (UIView header, IList<T> itemsSource, ElementBuilderDelegate<T> builder) : base (header)
		{
			ElementBuilder = builder;
			ItemsSource = itemsSource;
		}
		
		public SectionEx (UIView header, UIView footer, IList<T> itemsSource, ElementBuilderDelegate<T> builder) : base (header, footer)
		{
			ElementBuilder = builder;
			ItemsSource = itemsSource;
		}
		#endregion
		
		public ElementBuilderDelegate<T> ElementBuilder { get; private set; }
		
		private readonly Dictionary<T, Element> _map = new Dictionary<T, Element>();

		private IList<T> _itemsSource;
		public IList<T> ItemsSource
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
				
				Rebuild();
			}
		}
		
		protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Reset:
				this.Clear();
				break;
			case NotifyCollectionChangedAction.Add:
				var startingPos  = args.NewStartingIndex;
				var elements = new Element[args.NewItems.Count];
				int idx = 0;
				foreach (var item in args.NewItems.Cast<T>())
				{
					var element = ElementBuilder(item);
					_map[item] = element;

					elements[idx] = element;
					idx++;
				}

				var pos = startingPos >= 0 ? startingPos : Count;
				this.Insert(pos, UITableViewRowAnimation.Automatic, elements);
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (var item in args.OldItems.Cast<T>())
				{
					Element element;
					if (_map.TryGetValue(item, out element))
					{
						this.Remove(element);
						_map.Remove(item);
					}
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (args.NewItems != null && args.NewItems.Count > 0)
					this.Rebuild();
				break;
			}
		}
		
		public void Rebuild()
		{
			RemoveRange(0, Count);
			_map.Clear();
			foreach (var item in _itemsSource)
			{
				_map[item] = ElementBuilder(item);
			}
			
			AddAll(_map.Values);
		}
		
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			
			var collectionChanged = _itemsSource as INotifyCollectionChanged;
			if (collectionChanged != null)
				collectionChanged.CollectionChanged -= CollectionChangedOnCollectionChanged;
			_itemsSource = null;

			_map.Clear();
		}
	}
}

