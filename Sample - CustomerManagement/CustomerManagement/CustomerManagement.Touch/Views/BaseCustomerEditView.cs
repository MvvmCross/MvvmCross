using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using CustomerManagement.Core.Models;
using CustomerManagement.Core.ViewModels;
using MonoTouch.UIKit;
using CrossUI.Touch.Dialog.Elements;

namespace CustomerManagement.Touch.Views
{
    public class BaseCustomerEditView <TViewModel>
        : MvxDialogViewController
        , IMvxModalTouchView
        where TViewModel : BaseEditCustomerViewModel
    {
		public new TViewModel ViewModel {
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public BaseCustomerEditView()
            : base(UITableViewStyle.Grouped, null, true)
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
                ViewModel.CloseCommand.Execute(null);
            };

            if (ViewModel.Customer.PrimaryAddress == null)
                ViewModel.Customer.PrimaryAddress = new Address();
			
            this.Root = new RootElement("Customer Info")
                            {
                                new Section("Contact Info")
                                    {
                                        new StringElement("ID", ViewModel.Customer.ID ?? string.Empty),
                                        new EntryElement("Name", "Name").Bind(this, "Value Customer.Name"),
                                        new EntryElement("Website", "Website").Bind(this, "Value Customer.Website"),
                                        new EntryElement("Primary Phone", "Phone").Bind(this, "Value Customer.PrimaryPhone"),
                                    },
                                new Section("Primary Address")
                                    {
                                        new EntryElement("Address").Bind(this, "Value Customer.PrimaryAddress.Street1"),
                                        new EntryElement("Address2").Bind(this, "Value Customer.PrimaryAddress.Street2"),
                                        new EntryElement("City").Bind(this, "Value Customer.PrimaryAddress.City"),
                                        new EntryElement("State").Bind(this, "Value Customer.PrimaryAddress.State"),
                                        new EntryElement("Zip").Bind(this, "Value Customer.PrimaryAddress.Zip"),
                                    },
                            };
        }
		
        public virtual void Save()
        {
            ViewModel.DoSave();
        }
    }
}