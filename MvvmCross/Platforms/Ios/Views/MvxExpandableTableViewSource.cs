// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using CoreGraphics;
using Foundation;
using MvvmCross.Base;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Views.Expandable;
using MvvmCross.Platforms.Ios.Views.Expandable.Controllers;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    public abstract class MvxExpandableTableViewSource : MvxExpandableTableViewSource<IEnumerable<object>, object>
    {
        public MvxExpandableTableViewSource(UITableView tableView) : base(tableView)
        {
        }
    }

    public abstract class MvxExpandableTableViewSource<TItemSource, TItem> : MvxTableViewSource where TItemSource : IEnumerable<TItem>
    {
	    private SectionExpandableController _sectionExpandableController = new DefaultAllSectionsExpandableController();
        private readonly EventHandler _headerButtonCommand;

        private IEnumerable _itemsSource;
        public new IEnumerable ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (value is IEnumerable<TItemSource> itemsSource)
                {
                    _itemsSource = itemsSource;
                    _sectionExpandableController.ResetState();

                    ReloadTableData();
                }
                else
                {
                    throw new ArgumentException("value must be of type IEnumerable<TItemSource>");
                }
            }
        }

        private IEnumerable<TItemSource> CastItemSource => ItemsSource as IEnumerable<TItemSource>;

        public MvxExpandableTableViewSource(UITableView tableView) : base(tableView)
        {
            _headerButtonCommand = (sender, e) =>
            {
                var button = sender as UIButton;
                var section = button.Tag;

	            var changedSectionsResponse = _sectionExpandableController.ToggleState((int) section);
                tableView.ReloadData();

	            List<NSIndexPath> pathsToAnimate = new List<NSIndexPath>();

	            foreach (var changedSection in changedSectionsResponse.AllModifiedIndexes)
	            {
		            // Animate the section cells
		            nint rowCountForSection = RowsInSection(tableView, changedSection);

		            for (int i = 0; i < rowCountForSection; i++)
						pathsToAnimate.Add(NSIndexPath.FromItemSection(i, changedSection));
	            }

	            tableView.ReloadRows(pathsToAnimate.ToArray(), UITableViewRowAnimation.Automatic);
	            ScrollToSection(tableView, section);

	            if (changedSectionsResponse.CollapsedIndexes.Any())
		            OnSectionCollapsed(changedSectionsResponse.CollapsedIndexes);

	            if (changedSectionsResponse.ExpandedIndexes.Any())
		            OnSectionExpanded(changedSectionsResponse.ExpandedIndexes);
            };
        }

	    protected virtual void OnSectionExpanded(IEnumerable<int> sectionIndexes)
	    {
	    }

	    protected virtual void OnSectionCollapsed(IEnumerable<int> collapsedSectionIndexes)
	    {
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
            if (CastItemSource == null)
                return 0;
            // If the section is not colapsed return the rows in that section otherwise return 0
            if (CastItemSource.ElementAt((int)section).Any() && _sectionExpandableController.IsExpanded((int)section))
                return CastItemSource.ElementAt((int)section).Count();
            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            if (CastItemSource == null)
                return 0;

            return CastItemSource.Count();
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (CastItemSource == null)
                return null;

            return ((IEnumerable<object>)CastItemSource.ElementAt(indexPath.Section)).ElementAt(indexPath.Row);
        }

        protected object GetHeaderItemAt(nint section)
        {
            if (CastItemSource == null)
                return null;

            return CastItemSource.ElementAt((int)section);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var header = GetOrCreateHeaderCellFor(tableView, section);
	        var expandableHeaderCell = header as IExpandableHeaderCell;

	        if (expandableHeaderCell != null)
	        {
		        if (_sectionExpandableController.IsExpanded((int) section))
			        expandableHeaderCell.OnExpanded();
		        else
			        expandableHeaderCell.OnCollapsed();
	        }

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
		    get
            {
                return isAccordionExpandCollapseEnabled;
            }
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
        public HiddenHeaderButton(CGRect frame) : base(frame)
        {
        }
    }
}
