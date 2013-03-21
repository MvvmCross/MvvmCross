using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;

namespace CustomerManagement.Touch.Views
{
	/*
    public class CustomerListView 
        : MvxBindingTouchTableViewController<CustomerListViewModel>
    {
        public CustomerListView(MvxViewModelRequest request)
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

            // note that .AddCommand.Execute(null) is not used here - problem with ICommand and signed assemblies in Windows/VS
            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, e) => ViewModel.DoAdd()), false);
        }
        
        public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }		
    }
    */
}

