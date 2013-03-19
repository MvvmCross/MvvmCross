using System.Collections.Generic;
using Cirrious.Conference.Core.ViewModels.HomeViewModels;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Cirrious.Conference.UI.Touch.Views
{
    public class TwitterView
        : MvxViewController
    {
        private UIActivityIndicatorView _activityView;

		public new TwitterViewModel ViewModel {
			get { return (TwitterViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        FoldingTableViewController _tableView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //this.View.BackgroundColor = UIColor.Black;

            _activityView = new UIActivityIndicatorView(this.View.Frame);
            //Add(_activityView);
            //View.BringSubviewToFront(_activityView);

            _tableView = new FoldingTableViewController(new System.Drawing.RectangleF(0, 0, 320, 367), UITableViewStyle.Plain);
			var source = new TableSource(_tableView.TableView);

            this.AddBindings(new Dictionary<object, string>()
                                 {
                                     {source, "ItemsSource TweetsPlus"},
                                     //{_activityView, "{'Hidden':{'Path':'IsSearching','Converter':'InvertedVisibility'}}"},
				{_tableView, "Refreshing IsSearching;RefreshHeadCommand RefreshCommand;LastUpdatedText WhenLastUpdatedUtc,Converter=SimpleDate"},
                                 });

            _tableView.TableView.RowHeight = 100;
            _tableView.TableView.Source = source;
            _tableView.TableView.ReloadData();
            this.Add(_tableView.View);


            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShareGeneral()), false);
        }

		public class TableSource : MvxSimpleTableViewSource
		{
			public TableSource (UITableView tableView)
				: base(tableView, TweetCell3.Identifier, TweetCell3.Identifier)
			{
				tableView.RegisterNibForCellReuse(UINib.FromName(TweetCell3.Identifier, NSBundle.MainBundle), TweetCell3.Identifier);
			}

			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return 100.0f;
			}
		}
    }
}