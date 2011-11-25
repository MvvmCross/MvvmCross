using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
	class MGRootViewController : UITableViewController
	{
		internal MGDetailViewController detailViewController { get; set; }
		
		public MGRootViewController ()
		{
		}
		

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
		    ClearsSelectionOnViewWillAppear = false;
		    ContentSizeForViewInPopover = new SizeF(320, 600);
		}
		
		
		// Ensure that the view controller supports rotation and that the split view can therefore show in both portrait and landscape.
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		
		public void SelectFirstRow()
		{
			if (TableView.NumberOfSections() > 0 && TableView.NumberOfRowsInSection(0) > 0) {
				NSIndexPath indexPath = NSIndexPath.FromRowSection(0, 0);
				TableView.SelectRow(indexPath, true, UITableViewScrollPosition.Top);
				DidSelectRow(indexPath);
			}
		}
		
		
		UITableViewCell CellForRow(NSIndexPath indexPath)
		{
		    
		    string CellIdentifier = "CellIdentifier";
		    
		    // Dequeue or create a cell of the appropriate type.
		    UITableViewCell cell = TableView.DequeueReusableCell(CellIdentifier);
		    if (cell == null) {
		        cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
		        cell.Accessory = UITableViewCellAccessory.None;
		    }
		    
		    // Configure the cell.
		    cell.TextLabel.Text = string.Format("Row {0}", indexPath.Row);
		    return cell;
		}
		
		
		void DidSelectRow(NSIndexPath indexPath)
		{
			// When a row is selected, set the detail view controller's detail item to the item associated with the selected row.
		    detailViewController.DetailItem = string.Format("Row {0}", indexPath.Row);
		}
	}
}

