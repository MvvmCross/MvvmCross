using System.Collections.Generic;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace BestSellers.Touch.Views
{
	public class CategoryListView : MvxTableViewController
	{
		public new CategoryListViewModel ViewModel {
			get { return (CategoryListViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			Title = "Best Sellers";
			
			var source = new MvxStandardTableViewSource(
				TableView,
                UITableViewCellStyle.Default,
                new NSString("CategoryListView"),
				"TitleText DisplayName;SelectedCommand ShowCategoryCommand",
                UITableViewCellAccessory.DisclosureIndicator);
			
            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { source, "ItemsSource List" }
                    });

            TableView.Source = source;
            TableView.ReloadData();
        }
	}	
}

