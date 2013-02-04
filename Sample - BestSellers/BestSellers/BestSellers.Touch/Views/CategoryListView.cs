using System.Collections.Generic;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace BestSellers.Touch.Views
{
	public class CategoryListView : MvxBindingTableViewController
	{
		public new CategoryListViewModel ViewModel {
			get { return (CategoryListViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			Title = "Best Sellers";
			
			var source = new MvxSimpleBindableTableViewSource(
				TableView,
                UITableViewCellStyle.Default,
                new NSString("CategoryListView"),
                "{'TitleText':{'Path':'DisplayName'},'SelectedCommand':{'Path':'ShowCategoryCommand'}}",
                UITableViewCellAccessory.DisclosureIndicator);
			
            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { source, "{'ItemsSource':{'Path':'List'}}" }
                    });

            TableView.Source = source;
            TableView.ReloadData();
        }
	}	
}

