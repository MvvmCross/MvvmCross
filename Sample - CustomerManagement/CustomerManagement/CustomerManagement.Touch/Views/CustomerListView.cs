using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using CustomerManagement.Core.Models;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using CustomerManagement.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace CustomerManagement.Touch.Views
{
    public class CustomerListView 
        : MvxTableViewController
    {
		public new CustomerListViewModel ViewModel {
			get { return (CustomerListViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            Title = "Customers";

            var tableSource = new CustomerListTableViewSource(TableView);
            
            this.AddBindings(new Dictionary<object, string>()
                                 {
                                     {tableSource, "ItemsSource Customers; SelectionChangedCommand CustomerSelectedCommand"}
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
}

