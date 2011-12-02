using Android.Views;
using Cirrious.MvvmCross.Android.Views;
using CustomerManagement.Shared.Model;
using CustomerManagement.ViewModels;
using MonoDroid.Dialog;

namespace CustomerManagement.Droid.Views
{
    public class BaseCustomerEditView<TViewModel> : MvxDialogActivityView<TViewModel> 
        where TViewModel : BaseEditCustomerViewModel
    {
		int _whichMenu;
		
		public BaseCustomerEditView (int whichMenu)
		{
			_whichMenu = whichMenu;
		}
		
        EntryElement _nameEntry, _webEntry, _phoneEntry, _address1Entry, _address2Entry;
        EntryElement _cityEntry, _stateEntry, _zipEntry;

        protected override void OnViewModelChanged()
        {
            if (ViewModel.Customer.PrimaryAddress == null)
                ViewModel.Customer.PrimaryAddress = new Address();
			
#warning We could do simple data binding here instead			
            _nameEntry = new EntryElement("Name", ViewModel.Customer.Name ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
            _webEntry = new EntryElement("Website", ViewModel.Customer.Website ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
            _phoneEntry = new EntryElement("Primary Phone", ViewModel.Customer.PrimaryPhone ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);

            _address1Entry = new EntryElement("Address", ViewModel.Customer.PrimaryAddress.Street1 ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
            _address2Entry = new EntryElement("Address2", ViewModel.Customer.PrimaryAddress.Street2 ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
            _cityEntry = new EntryElement("City ", ViewModel.Customer.PrimaryAddress.City ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
            _stateEntry = new EntryElement("State ", ViewModel.Customer.PrimaryAddress.State ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
            _zipEntry = new EntryElement("ZIP", ViewModel.Customer.PrimaryAddress.Zip ?? string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow);
			
            this.Root = new RootElement("Customer Info")
                            {
                                new Section("Contact Info")
                                    {
                                        new StringElement("ID", ViewModel.Customer.ID ?? string.Empty),
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
            MenuInflater.Inflate(_whichMenu, menu);
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
            ViewModel.Customer.Name = _nameEntry.Value;
            ViewModel.Customer.Website = _webEntry.Value;
            ViewModel.Customer.PrimaryPhone = _phoneEntry.Value;

            ViewModel.Customer.PrimaryAddress.Street1 = _address1Entry.Value;
            ViewModel.Customer.PrimaryAddress.Street2 = _address2Entry.Value;
            ViewModel.Customer.PrimaryAddress.City = _cityEntry.Value;
            ViewModel.Customer.PrimaryAddress.State = _stateEntry.Value;
            ViewModel.Customer.PrimaryAddress.Zip = _zipEntry.Value;

            ViewModel.SaveCommand.Execute(null);
        }
    }
}