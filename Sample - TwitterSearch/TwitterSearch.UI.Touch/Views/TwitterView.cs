using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TwitterSearch.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace TwitterSearch.UI.Touch.Views
{
    public class TwitterView
        : MvxTableViewController
    {
		private UIActivityIndicatorView _activityView;

		public new TwitterViewModel ViewModel {
			get { return (TwitterViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
			_activityView = new UIActivityIndicatorView(this.View.Frame);
			Add(_activityView);
			View.BringSubviewToFront(_activityView);
			
			var source = new TableViewSource(TableView);

            var bindings = this.CreateBindingSet<TwitterView, TwitterViewModel>();
            bindings.Bind(source).To(vm => vm.Tweets);
            bindings.Bind(_activityView).For(v => v.Hidden).To(vm => vm.IsSearching).WithConversion("Visibility");
            bindings.Apply();
   
			TableView.Source = source;
			TableView.ReloadData();
        }

		public class TableViewSource : MvxTableViewSource
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