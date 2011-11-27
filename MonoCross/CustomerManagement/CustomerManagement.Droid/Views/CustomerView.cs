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
using CustomerManagement.Controllers;
using MonoCross.Navigation;
using MonoCross.Droid;
using MonoDroid.Dialog;

using CustomerManagement.Shared.Model;

using Cirrious.MonoCross.Extensions.ExtensionMethods;

namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "Customer Info", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class CustomerView : MXDialogActivityView<Customer>
    {
        public override void Render()
        {
            this.Root = new RootElement("Customer Info") {
                new Section("Contact Info") {
                    new StringElement("Name", Model.Name, (int)DroidResources.ElementLayout.dialog_labelfieldbelow),
                    new StringElement("Website", Model.Website, (int)DroidResources.ElementLayout.dialog_labelfieldbelow) {
						Click = (o, e) => { LaunchWeb(); },
					},
                    new StringElement("Primary Phone", Model.PrimaryPhone, (int)DroidResources.ElementLayout.dialog_labelfieldbelow) {
						Click = (o, e) => { LaunchDial(); },
					},
                },
                new Section("General Info") {
                    new StringMultilineElement("Address", Model.PrimaryAddress.ToString()) {
						Click = (o, e) => { LaunchMaps(); },
					},
                    new StringElement("Previous Orders ", Model.Orders.Count.ToString()),
                    new StringElement("Other Addresses ", Model.Addresses.Count.ToString()),
                    new StringElement("Contacts ", Model.Contacts.Count.ToString()),
                },
            };
        }
		
		void LaunchWeb()
		{
			Intent newIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(Model.Website));
			StartActivity(newIntent);
		}

		void LaunchMaps()
		{
            string googleAddress = Model.PrimaryAddress.ToString();
			googleAddress = System.Web.HttpUtility.UrlEncode(googleAddress);
			
			string url = string.Format("http://maps.google.com/maps?q={0}", googleAddress);
			Intent newIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
			StartActivity(newIntent);
		}
		
		void LaunchDial()
		{
			string phoneNumber = PhoneNumberUtils.FormatNumber(Model.PrimaryPhone);
			Intent newIntent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse("tel:" + phoneNumber));
			StartActivity(newIntent);
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
                this.Navigate<CustomerController>("Edit", new {customerId = Model.ID});
                return true;
            case Resource.Id.delete_customer:
                this.Navigate<CustomerController>("Delete", new {customerId = Model.ID});
                return true;
			}				
			return base.OnOptionsItemSelected (item);
		}
    }
}