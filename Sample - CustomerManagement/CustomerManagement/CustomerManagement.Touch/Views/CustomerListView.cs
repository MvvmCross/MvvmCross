using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Views;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.ViewModels;

using CustomerManagement;
using CustomerManagement.Core.Models;
using Cirrious.MvvmCross.Touch.Views;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Touch
{
    public class CustomerListView 
        : MvxTouchTableViewController<CustomerListViewModel>
        , IMvxServiceConsumer<IMvxBinder>
    {
        private readonly List<IMvxUpdateableBinding> _bindings;

        public CustomerListView(MvxShowViewModelRequest request)
            : base(request)
		{
            _bindings = new List<IMvxUpdateableBinding>();
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bindings.ForEach(x => x.Dispose());
            }

            base.Dispose(disposing);
        }

		public override void ViewDidLoad ()
		{
	 		base.ViewDidLoad ();
			
			Title = "Customers";

		    var binder = this.GetService<IMvxBinder>();
            var tableDelegate = new MvxBindableTableViewDelegate();
		    _bindings.AddRange(binder.Bind(ViewModel, tableDelegate, "{'ItemsSource':{'Path':'Customers'}}"));
		    tableDelegate.SelectionChanged += (sender, args) => ViewModel.CustomerSelectedCommand.Execute(args.AddedItems[0]);
            TableView.Delegate = tableDelegate;
            var tableSource = new CustomerListTableViewDataSource(TableView);
            _bindings.AddRange(binder.Bind(ViewModel, tableSource, "{'ItemsSource':{'Path':'Customers'}}"));
            TableView.DataSource = tableSource;
            TableView.ReloadData();

#warning iPad behaviour commented out				
			//if (MXTouchNavigation.MasterDetailLayout && Model.Count > 0)
			//{
				// we have two available panes, fill both (like the email application)
				//this.Navigate(string.Format("Customers/{0}", Model[0].ID));
			//}
			
			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, e) => ViewModel.AddCommand.Execute()), false);
		}

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            foreach (var binding in _bindings)
            {
                binding.Dispose();
            }
            _bindings.Clear();
        }
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}		
	}
}

