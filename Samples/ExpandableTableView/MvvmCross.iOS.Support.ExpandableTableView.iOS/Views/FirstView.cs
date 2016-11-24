using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Support.Views;
using MvvmCross.iOS.Views;
using UIKit;

namespace MvvmCross.iOS.Support.ExpandableTableView.iOS
{
	public class FirstView : MvxTableViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NavigationItem.LeftBarButtonItem = EditButtonItem;

			var source = new ExpandableTableSource(TableView)
			{
				UseAnimations = true,
				AddAnimation = UITableViewRowAnimation.Left,
				RemoveAnimation = UITableViewRowAnimation.Right
			};

			this.AddBindings(new Dictionary<object, string>
				{
					{source, "ItemsSource KittenGroups"}
				});

			TableView.Source = source;
			TableView.ReloadData();
		}

		public override void SetEditing(bool editing, bool animated)
		{
			TableView.AllowsMultipleSelectionDuringEditing = !Editing;

			base.SetEditing(editing, animated);
		}
	}

	public class ExpandableTableSource : MvxExpandableTableViewSource
	{
		public ExpandableTableSource(UITableView tableView) : base(tableView)
		{
			string nibName = "KittenCell";
			_cellIdentifier = new NSString(nibName);
			tableView.RegisterNibForCellReuse(UINib.FromName(nibName, NSBundle.MainBundle), CellIdentifier);


			string nibName2 = "HeaderCell";
			_headerCellIdentifier = new NSString(nibName2);
			tableView.RegisterNibForCellReuse(UINib.FromName(nibName2, NSBundle.MainBundle), HeaderCellIdentifier);
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 120f;
		}

		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.Delete;
		}

		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
		}

		protected override UITableViewCell GetOrCreateHeaderCellFor(UITableView tableView, nint section)
		{
			return tableView.DequeueReusableCell(HeaderCellIdentifier);
		}

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
			return tableView.DequeueReusableCell(CellIdentifier);
		}

		private readonly NSString _cellIdentifier;
		protected virtual NSString CellIdentifier => _cellIdentifier;

		private readonly NSString _headerCellIdentifier;
		protected virtual NSString HeaderCellIdentifier => _headerCellIdentifier;
	}
}
