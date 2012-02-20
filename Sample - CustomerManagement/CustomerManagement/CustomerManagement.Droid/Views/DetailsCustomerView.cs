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
using CustomerManagement.Core.ViewModels;


namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "Customer Info", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class DetailsCustomerView :  BaseView<DetailsCustomerViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_DetailsCustomerView);
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
                ViewModel.EditCommand.Execute();
                return true;
            case Resource.Id.delete_customer:
                ViewModel.DeleteCommand.Execute();
                return true;
			}				
			return base.OnOptionsItemSelected (item);
		}
    }
}