using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using MonoTouch;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.Foundation;

using MonoCross.Navigation;
using MonoCross.Touch;

using BestSellers;
using Touch.TestContainer;

namespace Touch.TestContainer.Views
{
	[MXTouchViewAttributes(ViewNavigationContext.Detail)]
	class BookView : MXTouchTableViewController<Book>
	{
		public BookView(): base()
		{
			TableView = new UITableView(new RectangleF(0, 0, 320, 480), UITableViewStyle.Grouped);
		}
		
		public override void Render ()
		{
			NavigationItem.Title = "Number " + Model.Rank;
			
			TableView.Source = new BookTableSource(Model);
			TableView.ReloadData();
		}
		
		private class BookTableSource : UITableViewSource
		{
			private Book model;
			private List<string> links;
			
			public BookTableSource (Book model)
			{
				this.model = model;
				links = new List<string>();
				
				if (!string.IsNullOrEmpty(model.BookReviewLink))
				{
					links.Add("View Book Review ...");
					links.Add(model.BookReviewLink);
				}
				if (!string.IsNullOrEmpty(model.FirstChapterLink))
				{
					links.Add("View First Chapter ...");
					links.Add(model.FirstChapterLink);
				}
				if (!string.IsNullOrEmpty(model.SundayReviewLink))
				{
					links.Add("View Sunday Review ...");
					links.Add(model.SundayReviewLink);
				}
				if (!string.IsNullOrEmpty(model.ArticleChapterLink))
				{
					links.Add("View Article Chapter ...");
					links.Add(model.ArticleChapterLink);
				}
			}
			
			public override int NumberOfSections (UITableView tableView)
			{
				return 1 + (links.Count / 2);
			}
			
			public override int RowsInSection (UITableView tableview, int section)
			{
				return 1;
			}
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return indexPath.Section == 0 ? 256 : 44;
			}
			
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0)
				{
					var cell = tableView.DequeueReusableCell("BookDetail");
					if (cell == null)
					{
						cell = new UITableViewCell(UITableViewCellStyle.Default, "BookDetail");
					}
					
					foreach (var subview in cell.ContentView.Subviews)
						subview.RemoveFromSuperview();
					
					string body = "<html><head><meta name=\"viewport\" content=\"initial-scale=1.0, user-scalable=no\"/></head><body style=\"-webkit-text-size-adjust:none;background:{0};margin:10px 15px 15px;font-family:helvatica,arial,sans-serif;font-size:16;\">{1}</body></html>";
					string html;
					if (model.Author == null)
					{
						html = "<b>No Information Available</b>";
					}
					else
					{
						html = String.Format("<img src='http://images.amazon.com/images/P/{0}.01.ZTZZZZZZ.jpg' align='left' style='max-height:125px;max-width:75px;padding:0px' />", model.ISBN10);
						html += model.Contributor + "<br/><br/>";
						html += "<b>Publisher</b>: " + model.Publisher + "<br/>";
						html += model.Description + "<br/><br/>";
						html += "<b>List Price</b>: $" + model.Price + "<br/><br/>";
					}
					
					UIWebView view = new UIWebView(new RectangleF(10, 10, 280, 232));
					UIView scrollView = view.Subviews.Last();
		            if (scrollView is UIScrollView)
		                ((UIScrollView)scrollView).ScrollEnabled = false;
					
					view.LoadHtmlString(String.Format(body, "#FFFFFF", html), new NSUrl(Environment.CurrentDirectory, true));
					
					cell.ContentView.AddSubview(view);
					
					return cell;
				}
				else
				{
					var cell = tableView.DequeueReusableCell("BookLink");
					if (cell == null)
					{
						cell = new UITableViewCell(UITableViewCellStyle.Default, "BookLink");
						cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
						cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
					}
					
					cell.TextLabel.Text = links[(indexPath.Section - 1) * 2];
					
					return cell;
				}
			}
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				UIApplication.SharedApplication.OpenUrl(new NSUrl(links[(indexPath.Section * 2) - 1]));
			}
		}
	}
}

