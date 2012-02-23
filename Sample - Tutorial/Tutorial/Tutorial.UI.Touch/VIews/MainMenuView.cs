using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels;
using Cirrious.MvvmCross.Touch.Views;

namespace Tutorial.UI.Touch.Views
{
    public class MainMenuView
        : MvxBindingTouchTableViewController<MainMenuViewModel>
    {
        public MainMenuView(MvxShowViewModelRequest request)
            : base(request)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Views";

            var tableDelegate = new MvxBindableTableViewDelegate();
            tableDelegate.SelectionChanged += (sender, args) => ViewModel.ShowItemCommand.Execute(args.AddedItems[0]);
            var tableSource = new TableViewDataSource(TableView);

            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { tableDelegate, "{'ItemsSource':{'Path':'Items'}}"},
                        { tableSource, "{'ItemsSource':{'Path':'Items'}}" }
                    });

            TableView.Delegate = tableDelegate;
            TableView.DataSource = tableSource;
            TableView.ReloadData();
        }

        #region Nested classes for the table

        public sealed class TableViewCell
            : MvxBindableTableViewCell
        {
            public const string BindingText = @"{'TitleText':{'Path':'Name'},'DetailText':{'Path':'FullName'}}";

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

        public class TableViewDataSource : MvxBindableTableViewDataSource
        {
            static readonly NSString CellIdentifier = new NSString("TableViewCell");

            public TableViewDataSource(UITableView tableView)
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

