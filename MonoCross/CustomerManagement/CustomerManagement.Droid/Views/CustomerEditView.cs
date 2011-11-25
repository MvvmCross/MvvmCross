using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MonoDroid.Dialog;
using MonoCross.Droid;
using MonoCross.Navigation;

using CustomerManagement;
using CustomerManagement.Shared.Model;


namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "Customer Changes", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class CustomerEditView : MXDialogActivityView<Customer>
    {
		EntryElement _nameEntry, _webEntry, _phoneEntry, _address1Entry, _address2Entry;
		EntryElement _cityEntry, _stateEntry, _zipEntry;
		
        public override void Render()
        {
			if (Model.PrimaryAddress == null)
				Model.PrimaryAddress = new Address();
			
	        _nameEntry = new EntryElement("Name", Model.Name ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
	        _webEntry = new EntryElement("Website",  Model.Website ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
	        _phoneEntry = new EntryElement("Primary Phone", Model.PrimaryPhone ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);

			_address1Entry = new EntryElement("Address", Model.PrimaryAddress.Street1 ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
			_address2Entry = new EntryElement("Address2", Model.PrimaryAddress.Street2 ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
	        _cityEntry = new EntryElement("City ", Model.PrimaryAddress.City ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
	        _stateEntry = new EntryElement("State ", Model.PrimaryAddress.State ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
	        _zipEntry = new EntryElement("ZIP", Model.PrimaryAddress.Zip ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
			
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.customer_edit_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.save_customer:
                    SaveCustomer();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
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
				this.Navigate(string.Format("Customers/{0}/UPDATE", Model.ID));
			else
				this.Navigate(string.Format("Customers/{0}/CREATE", Model.ID));
		}

    }
}