using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cirrious.MvvmCross.Android.Views;
using Cirrious.MvvmCross.Commands;
using CustomerManagement.Core.Models;
using CustomerManagement.Core.ViewModels;


#warning There are some problems here with databinding - need to improve the models up to INotifyPropertyChanged

namespace CustomerManagement.Droid.Views
{
    [Activity(Label = "Customer List", Icon = "@drawable/icon")]
    public class CustomerListView : BaseView<CustomerListViewModel>
    {
        /*
        class CustomerAdapter : ArrayAdapter<Customer>
	    {
            List<Customer> items;

            public CustomerAdapter(Context context, int textViewResourceId, List<Customer> items)
                : base(context, textViewResourceId, items)
            {
	            this.items = items;
	        }
	
	        public override View GetView(int position, View convertView, ViewGroup parent)
	        {
	            View v = convertView;
	            if (v == null) {
	                LayoutInflater li = (LayoutInflater)this.Context.GetSystemService(Context.LayoutInflaterService);
	                v = li.Inflate(Android.Resource.Layout.SimpleListItem2, null);
	            }

                Customer o = items[position];
	            if (o != null) {
	                TextView tt = (TextView)v.FindViewById(Android.Resource.Id.Text1);
	                if (tt != null)
	                    tt.Text = o.Name;
	                TextView bt = (TextView)v.FindViewById(Android.Resource.Id.Text2);
	                if (bt != null && o.Website != null)
	                    bt.Text = o.Website;
	            }
	            return v;
	        }
	    }
         */

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
            ViewModel.AddCommand.Execute();
        }
    }
}

