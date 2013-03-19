using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Views;
using Tutorial.Core.ViewModels.Lessons;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Tutorial.UI.Touch.Controls;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Tutorial.UI.Touch.Views
{
    public partial class PullToRefreshView
        : MvxViewController
    {
        public PullToRefreshView () 
            : base ("PullToRefreshView", null)
        {
        }
        
		public new PullToRefreshViewModel ViewModel {
			get { return (PullToRefreshViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            var foldingTvc = new FoldingTableViewController(TableViewHolder.Frame, UITableViewStyle.Grouped);
            
            // Perform any additional setup after loading the view, typically from a nib.
            var tableSource = new TableViewSource(foldingTvc.TableView);

            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { tableSource, "ItemsSource Emails" },
						{ foldingTvc, "RefreshHeadCommand RefreshHeadCommand; Refreshing IsRefreshingHead" },
                        { NumberOfEmailsLabel, "Text Emails.Count" }
                    });
                
            foldingTvc.TableView.Source = tableSource;
            foldingTvc.TableView.ReloadData();
			foldingTvc.TableView.RowHeight = 100.0f;
            
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

        public class TableViewSource : MvxTableViewSource
        {
			public TableViewSource(UITableView tableView) : base(tableView) { }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                var reuse = tableView.DequeueReusableCell(PullToRefreshTableCellView.Identifier);
                if (reuse != null)
                    return reuse;

                var toReturn = PullToRefreshTableCellView.LoadFromNib();
				
                return toReturn;
            }
			
			public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				return 100.0f;
			}
        }

        #endregion		
    }
}

