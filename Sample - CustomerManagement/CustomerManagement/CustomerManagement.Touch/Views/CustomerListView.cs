using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Views;
using CustomerManagement.Core.Models;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Touch.Views
{
    public class CustomerListView 
        : MvxBindingTouchTableViewController<CustomerListViewModel>
    {
        public CustomerListView(MvxShowViewModelRequest request)
            : base(request)
		{
		}

		public override void ViewDidLoad ()
		{
	 		base.ViewDidLoad ();
			
			Title = "Customers";

            var tableSource = new CustomerListTableViewSource(TableView);
			tableSource.SelectionChanged += (sender, args) => ViewModel.DoCustomerSelect((Customer)args.AddedItems[0]);
            
            this.AddBindings(new Dictionary<object, string>()
		                         {
		                             {tableSource, "{'ItemsSource':{'Path':'Customers'}}"}
		                         });

            TableView.Source = tableSource;
            TableView.ReloadData();

#warning iPad behaviour commented out				
			//if (MXTouchNavigation.MasterDetailLayout && Model.Count > 0)
			//{
				// we have two available panes, fill both (like the email application)
				//this.Navigate(string.Format("Customers/{0}", Model[0].ID));
			//}
			
			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, e) => ViewModel.DoAdd()), false);
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}		
	}
}

