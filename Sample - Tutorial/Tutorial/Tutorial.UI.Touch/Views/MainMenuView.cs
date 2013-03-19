using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Tutorial.UI.Touch.Views
{
    public class MainMenuView
        : MvxTableViewController
    {
        public MainMenuView()
        {
        }

		public new MainMenuViewModel ViewModel {
			get { return (MainMenuViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Views";

            var tableSource = new TableViewSource(TableView);
            
            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { tableSource, "ItemsSource Items; SelectionChangedCommand ShowItemCommand" }
                    });

            TableView.Source = tableSource;
            TableView.ReloadData();
        }

        #region Nested classes for the table

        public sealed class TableViewCell
            : MvxStandardTableViewCell
        {
			public const string BindingText = @"TitleText Name;DetailText FullName";

            // if you don't want to JSON text, then you can use MvxBindingDescription in C#, instead:
            //public static readonly MvxBindingDescription[] BindingDescriptions
            //    = new[]
            //      {
            //          new MvxBindingDescription()
            //              {
            //                  TargetName = "TitleText",
            //                  SourcePropertyPath = "Name"
            //              },
            //          new MvxBindingDescription()
            //              {
            //                  TargetName = "DetailText",
            //                  SourcePropertyPath = "FullName"
            //              },
            //      };

            public TableViewCell(UITableViewCellStyle cellStyle, NSString cellIdentifier)
                : base(BindingText, cellStyle, cellIdentifier)
            {
                Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }
        }

        public class TableViewSource : MvxTableViewSource
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

        #endregion
    }
}

