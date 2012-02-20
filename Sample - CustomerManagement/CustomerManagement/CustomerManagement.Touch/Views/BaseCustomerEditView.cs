using Cirrious.MvvmCross.Touch.Dialog;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using CustomerManagement.Core.Models;
using CustomerManagement.Core.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace CustomerManagement.Touch
{
    public class BaseCustomerEditView <TViewModel>
        : MvxTouchDialogViewController<TViewModel>
        where TViewModel : BaseEditCustomerViewModel
    {
        EntryElement _nameEntry;
        EntryElement _webEntry;
        EntryElement _phoneEntry;
        EntryElement _address1Entry;
        EntryElement _address2Entry;
        EntryElement _cityEntry;
        EntryElement _stateEntry;
        EntryElement _zipEntry;

        public BaseCustomerEditView(MvxShowViewModelRequest request)
            : base(request, UITableViewStyle.Grouped, null, true)
        {
        }
		
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			
#warning These navigation buttons aren't working currently :(			
            this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Done, null), false);
            this.NavigationItem.RightBarButtonItem.Clicked += delegate {
                                                                           ViewModel.SaveCommand.Execute();
            };
            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, null), false);
            this.NavigationItem.LeftBarButtonItem.Clicked += delegate {
                                                                          ViewModel.BackCommand.Execute();
            };

            if (ViewModel.Customer.PrimaryAddress == null)
                ViewModel.Customer.PrimaryAddress = new Address();
			
            _nameEntry = new EntryElement("Name", "Name", ViewModel.Customer.Name ?? string.Empty);
            _webEntry = new EntryElement("Website", "Website", ViewModel.Customer.Website ?? string.Empty);
            _phoneEntry = new EntryElement("Primary Phone", "Phone", ViewModel.Customer.PrimaryPhone ?? string.Empty);
            _address1Entry = new EntryElement("Address", "", ViewModel.Customer.PrimaryAddress.Street1 ?? string.Empty);
            _address2Entry = new EntryElement("Address2", "", ViewModel.Customer.PrimaryAddress.Street2 ?? string.Empty);
            _cityEntry = new EntryElement("City ", "", ViewModel.Customer.PrimaryAddress.City ?? string.Empty);
            _stateEntry = new EntryElement("State ", "", ViewModel.Customer.PrimaryAddress.State ?? string.Empty);
            _zipEntry = new EntryElement("ZIP", "", ViewModel.Customer.PrimaryAddress.Zip ?? string.Empty);
			
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
		
        void UpdateViewModel()
        {
            ViewModel.Customer.Name = _nameEntry.Value;
            ViewModel.Customer.Website = _webEntry.Value;
            ViewModel.Customer.PrimaryPhone = _phoneEntry.Value;
			
            ViewModel.Customer.PrimaryAddress.Street1 = _address1Entry.Value;
            ViewModel.Customer.PrimaryAddress.Street2 = _address2Entry.Value;
            ViewModel.Customer.PrimaryAddress.City = _cityEntry.Value;
            ViewModel.Customer.PrimaryAddress.State = _stateEntry.Value;
            ViewModel.Customer.PrimaryAddress.Zip = _zipEntry.Value;
        }
		
        public virtual void Save()
        {
            ViewModel.SaveCommand.Execute();
        }
    }
}