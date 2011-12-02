using System;
using System.Collections.Generic;

using MonoTouch.Dialog;
using MonoTouch.UIKit;

using MonoCross.Touch;
using MonoCross.Navigation;

using CustomerManagement;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.Touch
{
	[MXTouchViewAttributes(ViewNavigationContext.Detail)]
	public class CustomerEditView: MXTouchDialogView<Customer>
	{
		EntryElement _nameEntry;
		EntryElement _webEntry;
		EntryElement _phoneEntry;
		EntryElement _address1Entry;
		EntryElement _address2Entry;
		EntryElement _cityEntry;
		EntryElement _stateEntry;
		EntryElement _zipEntry;
		
		public CustomerEditView(): base(UITableViewStyle.Grouped, null, true)
		{
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Done, null), false);
			this.NavigationItem.RightBarButtonItem.Clicked += delegate {
				SaveCustomer();
			};
			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, null), false);
			this.NavigationItem.LeftBarButtonItem.Clicked += delegate {
				if (string.Equals("0", Model.ID))
					this.Navigate(string.Format("Customers", Model.ID));
				else
					this.Navigate(string.Format("Customers/{0}", Model.ID));
			};
		}
		
		public override void Render()
		{
			if (Model.PrimaryAddress == null)
				Model.PrimaryAddress = new Address();
			
	        _nameEntry = new EntryElement("Name", "Name", Model.Name ?? string.Empty);
	        _webEntry = new EntryElement("Website", "Website", Model.Website ?? string.Empty);
	        _phoneEntry = new EntryElement("Primary Phone", "Phone", Model.PrimaryPhone ?? string.Empty);
			_address1Entry = new EntryElement("Address", "", Model.PrimaryAddress.Street1 ?? string.Empty);
			_address2Entry = new EntryElement("Address2", "", Model.PrimaryAddress.Street2 ?? string.Empty);
	        _cityEntry = new EntryElement("City ", "", Model.PrimaryAddress.City ?? string.Empty);
	        _stateEntry = new EntryElement("State ", "", Model.PrimaryAddress.State ?? string.Empty);
	        _zipEntry = new EntryElement("ZIP", "", Model.PrimaryAddress.Zip ?? string.Empty);
			
			this.Root = new RootElement("Customer Info")
            {
                new Section("Contact Info")
                {
                    new StringElement("ID", Model.ID ?? string.Empty),
                    _nameEntry,
                    _webEntry,
                    _phoneEntry,
                },
                new Section("Primary Address")
                {
					_address1Entry,
					_address2Entry,
                    _cityEntry,
                    _stateEntry,
                    _zipEntry,
                },
            };
		}
		
		void SaveCustomer()
		{
			Model.Name = _nameEntry.Value;
			Model.Website = _webEntry.Value;
			Model.PrimaryPhone = _phoneEntry.Value;
			
			Model.PrimaryAddress.Street1 = _address1Entry.Value;
			Model.PrimaryAddress.Street2 = _address2Entry.Value;
			Model.PrimaryAddress.City = _cityEntry.Value;
			Model.PrimaryAddress.State = _stateEntry.Value;
			Model.PrimaryAddress.Zip = _zipEntry.Value;
			
			// Save
			if (string.Equals(Model.ID, "0"))
				this.Navigate(string.Format("Customers/{0}/CREATE", Model.ID));
			else
				this.Navigate(string.Format("Customers/{0}/UPDATE", Model.ID));
		}
	}
}

