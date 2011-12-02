using System;
using System.Collections.Generic;

using MonoTouch.Dialog;
using MonoTouch.UIKit;

using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;

using CustomerManagement;
using CustomerManagement.Shared.Model;
using MonoTouch.Foundation;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Touch
{
	[MvxTouchView(MvxTouchViewDisplayType.Detail)]
	public class CustomerView: MvxTouchDialogViewController<DetailsCustomerViewModel> 
	{
		public CustomerView (): base(UITableViewStyle.Grouped, null, true)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action, 
			    (sender, e) => { ActionMenu(); }), false);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			ResetDisplay();
		}
		
		private void ResetDisplay()
		{
#warning In a proper app, we need to do proper data binding			
			string addressString = ViewModel.Customer.PrimaryAddress != null ? ViewModel.Customer.PrimaryAddress.ToString() : string.Empty;
            this.Root = new RootElement("Customer Info")
            {
                new Section("Contact Info")
                {
                    new StringElement("ID", ViewModel.Customer.ID),
                    new StringElement("Name", ViewModel.Customer.Name ?? string.Empty),
                    new StringElement("Website", ViewModel.Customer.Website ?? string.Empty, delegate { LaunchWeb();}),
                    new StringElement("Primary Phone", ViewModel.Customer.PrimaryPhone ?? string.Empty, delegate { LaunchDial();})
                },
                new Section("General Info")
                {
					new StyledMultilineElement("Address", addressString, UITableViewCellStyle.Subtitle, delegate { LaunchMaps(); } ),
                    new StringElement("Previous Orders ", ViewModel.Customer.Orders != null ? ViewModel.Customer.Orders.Count.ToString() : string.Empty),
                    new StringElement("Other Addresses ", ViewModel.Customer.Addresses != null ? ViewModel.Customer.Addresses.Count.ToString() : string.Empty),
                    new StringElement("Contacts ", ViewModel.Customer.Contacts != null ? ViewModel.Customer.Contacts.Count.ToString() : string.Empty),
                },
            };
        }
		
		void ActionMenu()
		{
			//_actionSheet = new UIActionSheet("");
			UIActionSheet actionSheet = new UIActionSheet (
				"Customer Actions", null, "Cancel", "Delete Customer",
			     new string[] {"Change Customer"});
			actionSheet.Style = UIActionSheetStyle.Default;
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs args) {
				switch (args.ButtonIndex)
				{
				case 0: DeleteCustomer(); break;
				case 1: ChangeCustomer(); break;
				}
			};

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				actionSheet.ShowFromToolbar(NavigationController.Toolbar);
			else
				actionSheet.ShowFrom(NavigationItem.RightBarButtonItem, true);
		}
		
		void ChangeCustomer()
		{
			ViewModel.EditCommand.Execute();
		}
		
		void DeleteCustomer()
		{
			ViewModel.DeleteCommand.Execute();
		}
		
		void LaunchWeb()
		{
            ViewModel.ShowWebsiteCommand.Execute();
		}

		void LaunchMaps()
		{
            ViewModel.ShowOnMapCommand.Execute();
		}
		
		void LaunchDial()
		{
            ViewModel.CallCustomerCommand.Execute();
		}

		// dead code - left over from the previous sample
		void ViewOrders()
		{
		}
		
		// dead code - left over from the previous sample
		void NewOrder()
		{
		}
	}
}

