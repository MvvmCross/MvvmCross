using System;
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

            var tableSource = new TableViewSource(TableView);
            tableSource.SelectionChanged += (sender, args) => ViewModel.DoShowItem((Type)args.AddedItems[0]);
            
            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { tableSource, "{'ItemsSource':{'Path':'Items'}}" }
                    });

            TableView.Source = tableSource;
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

        #endregion
    }
}

