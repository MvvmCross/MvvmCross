using MvvmCross.iOS.Support.Views.ExpandableInternal;

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
    using CoreGraphics;

    public abstract class MvxExpandableTableViewSource : MvxExpandableTableViewSource<IEnumerable<object>, object>
    {
        public MvxExpandableTableViewSource(UITableView tableView) : base(tableView)
        {
        }

    }

    public abstract class MvxExpandableTableViewSource<TItemSource, TItem> : MvxTableViewSource where TItemSource : IEnumerable<TItem>
    {
	    private SectionExpandableController _sectionExpandableController = new DefaultAllSectionsExpandableController();
        private EventHandler _headerButtonCommand;

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
	            _sectionExpandableController.ResetState();

                ReloadTableData();
            }
        }

        public MvxExpandableTableViewSource(UITableView tableView) : base(tableView)
        {
            _headerButtonCommand = (sender, e) =>
            {
                var button = sender as UIButton;
                var section = button.Tag;

	            var changedSections = _sectionExpandableController.ToggleState((int) section).ToList();
                tableView.ReloadData();

	            List<NSIndexPath> pathsToAnimate = new List<NSIndexPath>();

	            foreach (var changedSection in changedSections)
	            {
		            // Animate the section cells
		            nint rowCountForSection = RowsInSection(tableView, changedSection);

		            for (int i = 0; i < rowCountForSection; i++)
						pathsToAnimate.Add(NSIndexPath.FromItemSection(i, changedSection));
	            }

	            tableView.ReloadRows(pathsToAnimate.ToArray(), UITableViewRowAnimation.Automatic);
	            ScrollToSection(tableView, section);
            };
        }

	    private void ScrollToSection(UITableView tableView, nint atIndex)
	    {
		    var sectionRect = tableView.RectForSection(atIndex);
		    sectionRect.Height = tableView.Frame.Height;
		    tableView.ScrollRectToVisible(sectionRect, true);
	    }

        protected override void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
	        _sectionExpandableController.ResetState();
			base.CollectionChangedOnCollectionChanged(sender, args);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (ItemsSource == null)
                return 0;
            // If the section is not colapsed return the rows in that section otherwise return 0
            if ((ItemsSource.ElementAt((int)section)).Any() && _sectionExpandableController.IsExpanded((int)section))
                return (ItemsSource.ElementAt((int)section)).Count();
            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            if (ItemsSource == null)
                return 0;

            return ItemsSource.Count();
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (ItemsSource == null)
                return null;

            return ((IEnumerable<object>)ItemsSource.ElementAt(indexPath.Section)).ElementAt(indexPath.Row);
        }

        protected object GetHeaderItemAt(nint section)
        {
            if (ItemsSource == null)
                return null;

            return ItemsSource.ElementAt((int)section);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var header = GetOrCreateHeaderCellFor(tableView, section);
            bool hasHiddenButton = false;

            foreach (var view in header.Subviews)
            {
                if (view is HiddenHeaderButton)
                {
                    var hiddenbutton = view as HiddenHeaderButton;
                    hiddenbutton.Tag = section;
                    hasHiddenButton = true;
                }
            }

            if (!hasHiddenButton)
            {
                // Create a button to make the header clickable
                var buttonFrame = header.Frame;
                buttonFrame.Width = UIScreen.MainScreen.ApplicationFrame.Width;
                var hiddenButton = CreateHiddenHeaderButton(buttonFrame, section);
                header.ContentView.AddSubview(hiddenButton);
            }

            // Set the header data context
            var bindable = header as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = GetHeaderItemAt(section);
            return header.ContentView;
        }

        private HiddenHeaderButton CreateHiddenHeaderButton(CGRect frame, nint tag)
        {
            var button = new HiddenHeaderButton(frame);
            button.Tag = tag;
            button.TouchUpInside += _headerButtonCommand;
            return button;
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

	    private bool isAccordionExpandCollapseEnabled;
	    public bool IsAccordionExpandCollapseEnabled
	    {
		    get { return isAccordionExpandCollapseEnabled; }
		    set
		    {
				if (isAccordionExpandCollapseEnabled == value)
					return;

				isAccordionExpandCollapseEnabled = value;

			    if (isAccordionExpandCollapseEnabled)
				    _sectionExpandableController = new AccordionSectionExpandableController();
			    else
				    _sectionExpandableController = new DefaultAllSectionsExpandableController();

				ReloadTableData();
			}
		} 
    }

    public class HiddenHeaderButton : UIButton
    {
        public HiddenHeaderButton(CGRect frame) : base(frame) { }
    }
}
