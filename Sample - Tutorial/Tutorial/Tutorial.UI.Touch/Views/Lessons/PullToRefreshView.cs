using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Views;
using Tutorial.Core.ViewModels.Lessons;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;
using Tutorial.UI.Touch.Controls;

namespace Tutorial.UI.Touch.Views
{
	public partial class PullToRefreshView 
        : MvxTouchViewController<PullToRefreshViewModel>
        , IMvxServiceConsumer<IMvxBinder>
	{
        private readonly List<IMvxUpdateableBinding> _bindings;

		public PullToRefreshView (MvxShowViewModelRequest request) 
			: base (request, "PullToRefreshView", null)
		{
            _bindings = new List<IMvxUpdateableBinding>();
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bindings.ForEach(x => x.Dispose());
            }

            base.Dispose(disposing);
        }		
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var foldingTvc = new FoldingTableViewController(TableViewHolder.Frame, UITableViewStyle.Grouped);
			
			// Perform any additional setup after loading the view, typically from a nib.
            var tableDelegate = new MvxBindableTableViewDelegate();
            var tableSource = new TableViewDataSource(foldingTvc.TableView);

            var binder = this.GetService<IMvxBinder>();
            _bindings.AddRange(binder.Bind(ViewModel, tableDelegate, "{'ItemsSource':{'Path':'Emails'}}"));
            _bindings.AddRange(binder.Bind(ViewModel, tableSource, "{'ItemsSource':{'Path':'Emails'}}"));
			_bindings.AddRange(binder.Bind(ViewModel, foldingTvc, "{'RefreshHeadCommand':{'Path':'RefreshHeadCommand'},'Refreshing':{'Path':'IsRefreshingHead'}}"));		
			_bindings.AddRange(binder.Bind(ViewModel, NumberOfEmailsLabel, "{'Text':{'Path':'Emails.Count'}}"));
			
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
            public static readonly MvxBindingDescription[] BindingDescriptions
                = new[]
                  {
                      new MvxBindingDescription()
                          {
                              TargetName = "TitleText",
                              SourcePropertyPath = "From"
                          },
                      new MvxBindingDescription()
                          {
                              TargetName = "DetailText",
                              SourcePropertyPath = "Header"
                          },
                  };

            // if you don't want to code the MvxBindingDescription in C#, then a string could instead be used instead:
            //public const string BindingText = @"{'TitleText':{'Path':'Name'},'DetailText':{'Path':'FullName'}}";

            public TableViewCell(UITableViewCellStyle cellStyle, NSString cellIdentifier)
                : base(BindingDescriptions, cellStyle, cellIdentifier)
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

