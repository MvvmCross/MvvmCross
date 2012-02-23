using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Views;
using Tutorial.Core.ViewModels.Lessons;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Tutorial.UI.Touch.Controls;

namespace Tutorial.UI.Touch.Views
{
    public partial class PullToRefreshView
        : MvxBindingTouchViewController<PullToRefreshViewModel>
	{
		public PullToRefreshView (MvxShowViewModelRequest request) 
			: base (request, "PullToRefreshView", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var foldingTvc = new FoldingTableViewController(TableViewHolder.Frame, UITableViewStyle.Grouped);
			
			// Perform any additional setup after loading the view, typically from a nib.
            var tableDelegate = new MvxBindableTableViewDelegate();
            var tableSource = new TableViewDataSource(foldingTvc.TableView);

		    AddBindings(
		        new Dictionary<object, string>()
		            {
                        { tableDelegate, "{'ItemsSource':{'Path':'Emails'}}" },
                        { tableSource, "{'ItemsSource':{'Path':'Emails'}}" },
			            { foldingTvc, "{'RefreshHeadCommand':{'Path':'RefreshHeadCommand'},'Refreshing':{'Path':'IsRefreshingHead'}}" },
                        { NumberOfEmailsLabel, "{'Text':{'Path':'Emails.Count'}}" }
		            });
                
			
            foldingTvc.TableView.Delegate = tableDelegate;
            foldingTvc.TableView.DataSource = tableSource;
            foldingTvc.TableView.ReloadData();
			
			Add(foldingTvc.View);			
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
		}
		
        #region Nested classes for the table

        public sealed class TableViewCell
            : MvxBindableTableViewCell
        {
            public const string BindingText = @"{'TitleText':{'Path':'From'},'DetailText':{'Path':'Header'}}";

            // if you don't want to code the MvxBindingDescription in JSON text, then C# could instead be used instead:
            //public static readonly MvxBindingDescription[] BindingDescriptions
            //    = new[]
            //      {
            //          new MvxBindingDescription()
            //              {
            //                  TargetName = "TitleText",
            //                  SourcePropertyPath = "From"
            //              },
            //          new MvxBindingDescription()
            //              {
            //                  TargetName = "DetailText",
            //                  SourcePropertyPath = "Header"
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

