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
using Android.Telephony;
using Cirrious.MvvmCross.Android.Views;
using CustomerManagement.ViewModels;
using MonoDroid.Dialog;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "Customer Info", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class CustomerView : MvxDialogActivityView<DetailsCustomerViewModel>
    {
        protected override void OnViewModelChanged()
        {
            this.Root = new RootElement("Customer Info") {
                new Section("Contact Info") {
                    new StringElement("Name", ViewModel.Customer.Name, (int)DroidResources.ElementLayout.dialog_labelfieldbelow),
                    new StringElement("Website", ViewModel.Customer.Website, (int)DroidResources.ElementLayout.dialog_labelfieldbelow) {
						Click = (o, e) => { LaunchWeb(); },
					},
                    new StringElement("Primary Phone", ViewModel.Customer.PrimaryPhone, (int)DroidResources.ElementLayout.dialog_labelfieldbelow) {
						Click = (o, e) => { LaunchDial(); },
					},
                },
                new Section("General Info") {
                    new StringMultilineElement("Address", ViewModel.Customer.PrimaryAddress.ToString()) {
						Click = (o, e) => { LaunchMaps(); },
					},
                    new StringElement("Previous Orders ", ViewModel.Customer.Orders.Count.ToString()),
                    new StringElement("Other Addresses ", ViewModel.Customer.Addresses.Count.ToString()),
                    new StringElement("Contacts ", ViewModel.Customer.Contacts.Count.ToString()),
                },
            };
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

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.customer_menu, menu);
			return true;
		}
		
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.change_customer:
                ViewModel.EditCommand.Execute(null);
                return true;
            case Resource.Id.delete_customer:
                ViewModel.DeleteCommand.Execute(null);
                return true;
			}				
			return base.OnOptionsItemSelected (item);
		}
    }
}