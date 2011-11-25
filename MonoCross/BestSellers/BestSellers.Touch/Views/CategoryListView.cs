using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.UIKit;
using MonoTouch.Dialog;

using Touch.TestContainer;

using MonoCross.Navigation;
using MonoCross.Touch;

using BestSellers;
using MonoTouch.Foundation;

namespace Touch.TestContainer.Views
{
	[MXTouchViewAttributes(ViewNavigationContext.Master)]
	public class CategoryListView : MXTouchTableViewController<CategoryList>
	{
		public CategoryListView()
		{
		}
		
		public override void Render ()
		{
			Title = "Best Sellers";
			
			TableView.Delegate = new TableViewDelegate(this, Model);
			TableView.DataSource = new TableViewDataSource(Model);
			TableView.ReloadData();
		}
		
		private class TableViewDelegate : UITableViewDelegate
	    {
	        private CategoryList list;
			private CategoryListView parent;
			
	        public TableViewDelegate(CategoryListView parent, CategoryList list)
	        {
				this.parent = parent;
	            this.list = list;
	        }
	        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
	        {
				string uri = list[indexPath.Row].ListNameEncoded;
				parent.Navigate(uri);
	        }
	    }
	 
	    private class TableViewDataSource : UITableViewDataSource
	    {
	        static NSString kCellIdentifier = new NSString ("MyIdentifier");
	        private CategoryList list;
	
			public TableViewDataSource (CategoryList list)
	        {
	            this.list = list;
	        }
	
			public override int RowsInSection (UITableView tableview, int section)
	        {
	            return list.Count;
	        }
	
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (kCellIdentifier);
	            if (cell == null)
				{
		            cell = new UITableViewCell (UITableViewCellStyle.Value1, kCellIdentifier);
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
	            }
	            cell.TextLabel.Text = list[indexPath.Row].DisplayName;
	            return cell;
	        }
			public override string TitleForHeader (UITableView tableView, int section)
			{
				return "Best Seller Categories";
			}
			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}
		}
	}
	
}

