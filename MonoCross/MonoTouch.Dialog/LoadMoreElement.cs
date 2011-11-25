//
// This cell does not perform cell recycling, do not use as
// sample code for new elements. 
//
using System;
using System.Drawing;
using System.Threading;
using MonoTouch.CoreFoundation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.Dialog
{
	public class LoadMoreElement : Element, IElementSizing
	{
		public string NormalCaption { get; set; }
		public string LoadingCaption { get; set; }
		
		Action<LoadMoreElement> tapped = null;
		
		UITableViewCell cell;
		UIActivityIndicatorView activityIndicator;
		UILabel caption;
		UIFont font;
		
		public LoadMoreElement (string normalCaption, string loadingCaption, Action<LoadMoreElement> tapped) : this (normalCaption, loadingCaption, tapped, UIFont.BoldSystemFontOfSize (16), UIColor.Black)
		{
		}
		
		public LoadMoreElement (string normalCaption, string loadingCaption, Action<LoadMoreElement> tapped, UIFont font, UIColor textColor) : base ("")
		{
			this.NormalCaption = normalCaption;
			this.LoadingCaption = loadingCaption;
			this.tapped = tapped;
			this.font = font;
			
			cell = new UITableViewCell (UITableViewCellStyle.Default, "loadMoreElement");
			
			activityIndicator = new UIActivityIndicatorView () {
				ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray,
				Hidden = true
			};
			activityIndicator.StopAnimating ();
			
			caption = new UILabel () {
				Font = font,
				Text = this.NormalCaption,
				TextColor = textColor,
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Center,
				AdjustsFontSizeToFitWidth = false,
			};
			
			Layout ();
			
			cell.ContentView.AddSubview (caption);
			cell.ContentView.AddSubview (activityIndicator);
		}
		
		public bool Animating {
			get {
				return activityIndicator.IsAnimating;
			}
			set {
				if (value){
					caption.Text = LoadingCaption;
					activityIndicator.Hidden = false;
					activityIndicator.StartAnimating ();
				} else {
					activityIndicator.StopAnimating ();
					activityIndicator.Hidden = true;
					caption.Text = NormalCaption;
				}
				Layout ();
			}
		}
				
		public override UITableViewCell GetCell (UITableView tv)
		{
			Layout ();
			return cell;
		}
				
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			tableView.DeselectRow (path, true);
			
			if (Animating)
				return;
			
			if (tapped != null){
				Animating = true;
				Layout ();
			}
			
			if (tapped != null)
				tapped (this);
		}
		
		SizeF GetTextSize ()
		{
			return new NSString (caption.Text).StringSize (font, UIScreen.MainScreen.Bounds.Width, UILineBreakMode.TailTruncation);
		}
		
		const int pad = 10;
		const int isize = 20;
		
		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return GetTextSize ().Height + 2*pad;
		}
		
		void Layout ()
		{
			var sbounds = cell.ContentView.Bounds;

			var size = GetTextSize ();
			
			if (!activityIndicator.Hidden)
				activityIndicator.Frame = new RectangleF ((sbounds.Width-size.Width)/2-isize*2, pad, isize, isize);

			caption.Frame = new RectangleF (10, pad, sbounds.Width-20, size.Height);
		}
		
		public UITextAlignment Alignment {
			get {
				return caption.TextAlignment;
			}
			set {
				caption.TextAlignment = value;
			}
		}
		public UITableViewCellAccessory Accessory {
			get {
				return cell.Accessory;
			}
			set {
				cell.Accessory = value;
			}
		}
	}
}

