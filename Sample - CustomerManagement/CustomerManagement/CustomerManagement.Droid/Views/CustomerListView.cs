using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using CustomerManagement.Core.Models;
using CustomerManagement.Core.ViewModels;


namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "Customer List", Icon = "@drawable/icon")]
    public class CustomerListView : BaseView<CustomerListViewModel>
    {
        public override bool OnCreateOptionsMenu (IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.customer_list_menu, menu);
            return true;
        }
        
        public override bool OnOptionsItemSelected (IMenuItem item)
        {
            switch (item.ItemId)
            {
            case Resource.Id.add_customer:
                AddCustomer();
                return true;
            }
            return base.OnOptionsItemSelected (item);
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_CustomerListView);
        }
        
        void AddCustomer()
        {
            ViewModel.AddCommand.Execute(null);
        }
    }
}
