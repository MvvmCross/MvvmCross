using System.Collections.Generic;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace BestSellers.Touch.Views
{
	public class CategoryListView : MvxBindingTouchTableViewController<CategoryListViewModel>
	{
		public CategoryListView(MvxShowViewModelRequest request)
            : base(request)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			Title = "Best Sellers";

            var tableSource = new TableViewSource(TableView);
            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { tableSource, "{'ItemsSource':{'Path':'List'}}" }
                    });

            TableView.Source = tableSource;
            TableView.ReloadData();
        }

        public sealed class TableViewCell
            : MvxBindableTableViewCell
        {
            public const string BindingText = @"{'TitleText':{'Path':'DisplayName'},'SelectedCommand':{'Path':'ShowCategoryCommand'}}";

            public TableViewCell(UITableViewCellStyle cellStyle, NSString cellIdentifier)
                : base(BindingText, cellStyle, cellIdentifier)
            {
                Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }
        }

        public class TableViewSource : MvxBindableTableViewSource
        {
            static readonly NSString CellIdentifier = new NSString("TableViewCell");

            public TableViewSource(UITableView tableView)
                : base(tableView)
            {
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                var reuse = tableView.DequeueReusableCell(CellIdentifier);
                if (reuse != null)
                    return reuse;

                var toReturn = new TableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier);
                return toReturn;
            }
        }
	}	
}

