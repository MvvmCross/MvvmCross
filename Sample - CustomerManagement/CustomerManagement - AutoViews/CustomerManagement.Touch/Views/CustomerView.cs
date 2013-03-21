using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace CustomerManagement.Touch.Views
{
	/*
	public class CustomerView: MvxTouchDialogViewController<DetailsCustomerViewModel> 
	{
        public CustomerView(MvxViewModelRequest request)
            : base(request, UITableViewStyle.Grouped, null, true)
        {
        }
	
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            // so here we want to bind the table to the viewmodel... how?


            // need to bind the table...


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
            
			string addressString = ViewModel.Customer.PrimaryAddress != null ? ViewModel.Customer.PrimaryAddress.ToString() : string.Empty;
            this.Root = new RootElement("Customer Info")
            {
                new Section("Contact Info")
                {
                    new StringElement("ID").Bind(this, "{'Value':{'Path':'Customer.ID'}}"),
                    new StringElement("Name").Bind(this, "{'Value':{'Path':'Customer.Name'}}"),
                    new StringElement("Website").Bind(this, "{'Value':{'Path':'Customer.Website'},'SelectedCommand':{'Path':'ShowWebsiteCommand'}}"),
                    new StringElement("Primary Phone").Bind(this, "{'Value':{'Path':'Customer.PrimaryPhone'},'SelectedCommand':{'Path':'CallCustomerCommand'}}"),
                },
                new Section("General Info")
                {
					new StyledMultilineElement("Address").Bind(this, "{'Value':{'Path':'Customer.PrimaryAddress'},'SelectedCommand':{'Path':'ShowOnMapCommand'}}"),
                    //new StringElement("Previous Orders ", ViewModel.Customer.Orders != null ? ViewModel.Customer.Orders.Count.ToString() : string.Empty),
                    //new StringElement("Other Addresses ", ViewModel.Customer.Addresses != null ? ViewModel.Customer.Addresses.Count.ToString() : string.Empty),
                    //new StringElement("Contacts ", ViewModel.Customer.Contacts != null ? ViewModel.Customer.Contacts.Count.ToString() : string.Empty),
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
			ViewModel.DoEdit();
		}
		
		void DeleteCustomer()
		{
			ViewModel.DoDelete();
		}		
	}
	*/
}

