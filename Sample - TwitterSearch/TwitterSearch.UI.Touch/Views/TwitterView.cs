using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Touch.Views
{
    public class TwitterView
        : MvxBindingTouchTableViewController<TwitterViewModel>
    {
		private UIActivityIndicatorView _activityView;
		
        public TwitterView(MvxShowViewModelRequest request)
            : base(request)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
			_activityView = new UIActivityIndicatorView(this.View.Frame);
			Add(_activityView);
			View.BringSubviewToFront(_activityView);
			
			var source = new TableViewSource(TableView);

            this.AddBindings(new Dictionary<object, string>()
		                         {
		                             {source, "ItemsSource Tweets"},
									 {_activityView, "Hidden IsSearching, Converter=Visibility"},
		                         });
            TableView.Source = source;
			TableView.ReloadData();
        }

		public class TableViewSource : MvxBindableTableViewSource
		{
			public TableViewSource(UITableView tableView)
				: base(tableView)
			{
				tableView.RegisterNibForCellReuse(UINib.FromName("TwitterCell", NSBundle.MainBundle), TwitterCell.CellIdentifier);
			}
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				var item = GetItemAt(indexPath);
				return TwitterCell.CellHeight(item);
			}
			
			protected override UITableViewCell GetOrCreateCellFor (UITableView tableView, NSIndexPath indexPath, object item)
			{
				var cellName = TwitterCell.CellIdentifier;				
				return (UITableViewCell)tableView.DequeueReusableCell(cellName, indexPath);
			}    
		}
	}
}