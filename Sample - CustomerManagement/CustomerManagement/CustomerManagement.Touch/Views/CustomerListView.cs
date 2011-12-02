using System;
using System.Collections.Generic;

using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

using Cirrious.MvvmCross.ViewModel;

using CustomerManagement;
using CustomerManagement.Shared.Model;
using Cirrious.MvvmCross.Touch.Views;
using CustomerManagement.ViewModels;
using Cirrious.MvvmCross.Commands;

namespace CustomerManagement.Touch
{
	[MvxTouchView(MvxTouchViewDisplayType.Master)]
	public class CustomerListView : MvxTouchTableViewController<CustomerListViewModel>
	{
		public CustomerListView ()
		{
		}
		
		public override void ViewDidLoad ()
		{
	 		base.ViewDidLoad ();
			
			Title = "Customers";
			
			TableView.Delegate = new TableViewDelegate(this, ViewModel.Customers);
			TableView.DataSource = new TableViewDataSource(ViewModel.Customers);
			TableView.ReloadData();

#warning iPad behaviour commented out				
			//if (MXTouchNavigation.MasterDetailLayout && Model.Count > 0)
			//{
				// we have two available panes, fill both (like the email application)
				//this.Navigate(string.Format("Customers/{0}", Model[0].ID));
			//}
			
#warning in a proper app we need proper databinding			
		 	ViewModel.PropertyChanged += (sender, e) => {
				// not efficient but OK for demo
				TableView.Delegate = new TableViewDelegate(this, ViewModel.Customers);
			    TableView.DataSource = new TableViewDataSource(ViewModel.Customers);
				TableView.ReloadData();
			};
			
			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add, 
			    (sender, e) => { NewCustomer(); }), false);
		}
		
		public void NewCustomer()
		{
			ViewModel.AddCommand.Execute();
		}
		
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		private class TableViewDelegate : UITableViewDelegate
	    {
			private CustomerListView _parent;
	        private List<Customer> _clientList;

			public TableViewDelegate(CustomerListView parent, List<Customer> list)
	        {
				_parent = parent;
	            _clientList = list;
	        }
	        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
	        {
				Customer client = _clientList[indexPath.Row];
				var selectionChanged = MvxSimpleSelectionChangedEventArgs.JustAddOneItem(client);
            	_parent.ViewModel.SelectionChanged.Execute(selectionChanged);
	        }
	    }
	 
	    private class TableViewDataSource : UITableViewDataSource
	    {
	        static NSString kCellIdentifier = new NSString ("ClientCell");
	        private List<Customer> _list;
	
			public TableViewDataSource (List<Customer> list)
	        {
	            this._list = list;
	        }
			public override int RowsInSection (UITableView tableview, int section)
	        {
	            return _list.Count;
	        }
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (kCellIdentifier);
	            if (cell == null) {
		            cell = new UITableViewCell (UITableViewCellStyle.Subtitle, kCellIdentifier);
#warning iPad functionality removed					
					//if (!MXTouchNavigation.MasterDetailLayout)
						cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
					//else
					//	cell.Accessory = UITableViewCellAccessory.None;
	            }
	            cell.TextLabel.Text = _list[indexPath.Row].Name;
				cell.DetailTextLabel.Text = _list[indexPath.Row].Website;
	            return cell;
	        }
			public override string TitleForHeader (UITableView tableView, int section)
			{
				return string.Empty;
			}
			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}
		}
	}
}

