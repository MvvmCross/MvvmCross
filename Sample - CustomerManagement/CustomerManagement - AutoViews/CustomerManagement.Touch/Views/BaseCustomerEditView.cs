using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Views;
using CustomerManagement.AutoViews.Core.Models;
using CustomerManagement.AutoViews.Core.ViewModels;
using MonoTouch.UIKit;

namespace CustomerManagement.Touch.Views
{
	/*
    public class BaseCustomerEditView <TViewModel>
        : MvxTouchDialogViewController<TViewModel>
        , IMvxModalTouchView
        where TViewModel : BaseEditCustomerViewModel
    {
        public BaseCustomerEditView(MvxViewModelRequest request)
            : base(request, UITableViewStyle.Grouped, null, true)
        {
        }
		
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			
            this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Done, null), false);
            this.NavigationItem.RightBarButtonItem.Clicked += delegate {
                                                                           ViewModel.DoSave();
            };
            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, null), false);
            this.NavigationItem.LeftBarButtonItem.Clicked += delegate {
                                                                          ViewModel.DoClose();
            };

            if (ViewModel.Customer.PrimaryAddress == null)
                ViewModel.Customer.PrimaryAddress = new Address();
			
            this.Root = new RootElement("Customer Info")
                            {
                                new Section("Contact Info")
                                    {
                                        new StringElement("ID", ViewModel.Customer.ID ?? string.Empty),
                                        new EntryElement("Name", "Name").Bind(this, "{'Value':{'Path':'Customer.Name'}}"),
                                        new EntryElement("Website", "Website").Bind(this, "{'Value':{'Path':'Customer.Website'}}"),
                                        new EntryElement("Primary Phone", "Phone").Bind(this, "{'Value':{'Path':'Customer.PrimaryPhone'}}"),
                                    },
                                new Section("Primary Address")
                                    {
                                        new EntryElement("Address").Bind(this, "{'Value':{'Path':'Customer.PrimaryAddress.Street1'}}"),
                                        new EntryElement("Address2").Bind(this, "{'Value':{'Path':'Customer.PrimaryAddress.Street2'}}"),
                                        new EntryElement("City").Bind(this, "{'Value':{'Path':'Customer.PrimaryAddress.City'}}"),
                                        new EntryElement("State").Bind(this, "{'Value':{'Path':'Customer.PrimaryAddress.State'}}"),
                                        new EntryElement("Zip").Bind(this, "{'Value':{'Path':'Customer.PrimaryAddress.Zip'}}"),
                                    },
                            };
        }
		
        public virtual void Save()
        {
            ViewModel.DoSave();
        }
    }
    */
}