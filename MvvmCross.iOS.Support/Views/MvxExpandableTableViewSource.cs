namespace MvvmCross.iOS.Support.Views
{
    using Foundation;
    using Binding.ExtensionMethods;
    using Binding.iOS.Views;
    using MvvmCross.Platform.Core;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using UIKit;

    public abstract class MvxExpandableTableViewSource<TItemSource, TItem> : MvxTableViewSource where TItemSource : List<TItem>
    {
	{
        /// <summary>
        /// Indicates which sections are expanded.
        /// </summary>
        private bool[] _isCollapsed;


		private IEnumerable<TItemSource> _itemsSource;
		new public IEnumerable<TItemSource> ItemsSource
		{
			get
			{
				return _itemsSource;
			}
			set
			{
				_itemsSource = value;
				_isCollapsed = new bool[ItemsSource.Count()];

				for (var i = 0; i < _isCollapsed.Length; i++)
					_isCollapsed[i] = true;
				ReloadTableData();
			}
		}

        public MvxExpandableTableViewSource(UITableView tableView) : base(tableView)
        {
        }


        protected override void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            // When the collection is changed collapse all sections
            _isCollapsed = new bool[ItemsSource.Count()];


            for (var i = 0; i < _isCollapsed.Length; i++)
                _isCollapsed[i] = true;


            base.CollectionChangedOnCollectionChanged(sender, args);
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
			// If the section is not colapsed return the rows in that section otherwise return 0
			if ((ItemsSource?.ElementAt((int)section)).Any() && !_isCollapsed[(int)section])
				return (ItemsSource.ElementAt((int)section)).Count();
			return 0;
        }


        public override nint NumberOfSections(UITableView tableView)
        {
            return ItemsSource?.Count() ?? 0;
        }


        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ((IEnumerable<object>)ItemsSource?.ElementAt(indexPath.Section)).ElementAt(indexPath.Row);
        }


        protected object GetHeaderItemAt(nint section)
        {
            return ItemsSource?.ElementAt((int)section);
        }


        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var header = GetOrCreateHeaderCellFor(tableView, section);


            // Create a button to make the header clickable
            UIButton hiddenButton = new UIButton(header.Frame);
            hiddenButton.TouchUpInside += EventHandler(tableView, section);
            header.AddSubview(hiddenButton);


            // Set the header data context
            var bindable = header as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = GetHeaderItemAt(section);
            return header;
        }


        private EventHandler EventHandler(UITableView tableView, nint section)
        {
            return (sender, e) =>
            {
                // Toggle the is collapsed
                _isCollapsed[(int)section] = !_isCollapsed[(int)section];
                tableView.ReloadData();


                // Animate the section cells
                var paths = new NSIndexPath[RowsInSection(tableView, section)];
                for (int i = 0; i < paths.Length; i++)
                {
                    paths[i] = NSIndexPath.FromItemSection(i, section);
                }


                tableView.ReloadRows(paths, UITableViewRowAnimation.Automatic);
            };
        }


        public override void HeaderViewDisplayingEnded(UITableView tableView, UIView headerView, nint section)
        {
            var bindable = headerView as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = null;
        }


        /// <summary>
        /// This is needed to show the header view. Should be overriden by sources that inherit from this.
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 44; // Default value.
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return base.GetCell(tableView, indexPath);
        }


        /// <summary>
        /// Gets the cell used for the header
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        protected abstract UITableViewCell GetOrCreateHeaderCellFor(UITableView tableView, nint section);

        protected abstract override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item);
    }
}
