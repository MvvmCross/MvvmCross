using System;
using System.Collections.Generic;

using MonoTouch.Dialog;
using MonoTouch.UIKit;

using MonoCross.Touch;
using MonoCross.Navigation;

using CustomerManagement;
using CustomerManagement.Shared.Model;
using MonoTouch.Foundation;

namespace CustomerManagement.Touch
{
	[MXTouchViewAttributes(ViewNavigationContext.Detail)]
	public class CustomerView: MXTouchDialogView<Customer> 
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

	    public override void Render()
		{
			string addressString = Model.PrimaryAddress != null ? Model.PrimaryAddress.ToString() : string.Empty;
            this.Root = new RootElement("Customer Info")
            {
                new Section("Contact Info")
                {
                    new StringElement("ID", Model.ID),
                    new StringElement("Name", Model.Name ?? string.Empty),
                    new StringElement("Website", Model.Website ?? string.Empty, delegate { LaunchWeb();}),
                    new StringElement("Primary Phone", Model.PrimaryPhone ?? string.Empty, delegate { LaunchDial();})
                },
                new Section("General Info")
                {
					new StyledMultilineElement("Address", addressString, UITableViewCellStyle.Subtitle, delegate { LaunchMaps(); } ),
                    new StringElement("Previous Orders ", Model.Orders != null ? Model.Orders.Count.ToString() : string.Empty),
                    new StringElement("Other Addresses ", Model.Addresses != null ? Model.Addresses.Count.ToString() : string.Empty),
                    new StringElement("Contacts ", Model.Contacts != null ? Model.Contacts.Count.ToString() : string.Empty),
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
			this.Navigate(string.Format("Customers/{0}/EDIT", Model.ID));
		}
		
		void DeleteCustomer()
		{
			var alert = new UIAlertView ("Delete Client", "Are you sure?",  null, "OK", "Cancel");
			alert.Show ();
		    alert.Clicked += (sender, buttonArgs) => {
				if (buttonArgs.ButtonIndex == 0)
				{
					this.Navigate(string.Format("Customers/{0}/DELETE", Model.ID));
				}
			};    
		}
		
		void LaunchWeb()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(Model.Website));
		}

		void LaunchMaps()
		{
			string googleAddress = string.Format("{0} {1}\n{2}, {3}  {4}",
						Model.PrimaryAddress.Street1, Model.PrimaryAddress.Street2, 
						Model.PrimaryAddress.City, Model.PrimaryAddress.State, Model.PrimaryAddress.Zip);
			
			googleAddress = System.Web.HttpUtility.UrlEncode(googleAddress);
			
			string url = string.Format("http://maps.google.com/maps?q={0}", googleAddress);

			UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
		}
		
		void LaunchDial()
		{
			string url = string.Format("tel:{0}", Model.PrimaryPhone);
			UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
		}
 
		void ViewOrders()
		{
		}
		
		void NewOrder()
		{
		}
	}
}

