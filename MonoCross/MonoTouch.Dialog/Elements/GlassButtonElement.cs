using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

namespace MonoTouch.Dialog
{
	public class GlassButtonElement: Element
	{
		static NSString Key = new NSString ("GlassButtonElement");

		public UIColor HighlightedColor, NormalColor, DisabledColor;
		public UIColor HighlightedTextColor, NormalTextColor, DisabledTextColor;

		GlassButton button;
		NSAction tapped = null;
		
		public GlassButtonElement (string caption, NSAction tapped) : base (caption)
		{
			this.tapped = tapped;
			
			NormalColor = UIColor.Gray;
			NormalTextColor = UIColor.Black;

			HighlightedColor = UIColor.Blue;
			HighlightedTextColor = UIColor.White;

			DisabledColor = UIColor.Gray;
			DisabledTextColor = UIColor.DarkGray;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (Key);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, Key);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				cell.Frame = new RectangleF(cell.Frame.X, cell.Frame.Y, tv.Frame.Width, cell.Frame.Height);
			} else {
				RemoveTag (cell, 1);
			}
			
			if (button == null) {
				RectangleF frame = cell.Frame;
				// after the first run the Y offset of the cell is unknown, 
				frame.Y = 0;
				frame.Inflate(-8, 0);

				button = new GlassButton(frame);
				button.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin;
				button.Font = UIFont.BoldSystemFontOfSize (22);
				button.TouchUpInside += (o, e) => tapped.Invoke();
			} else {
				button.RemoveFromSuperview();
			}
			
			button.SetTitle(this.Caption, UIControlState.Normal);
			button.SetTitleColor(UIColor.White, UIControlState.Normal);
			button.BackgroundColor = UIColor.Clear;
			button.HighlightedColor = this.HighlightedColor;
			button.NormalColor = this.NormalColor;
			button.DisabledColor = this.DisabledColor;
			
			
			// note: button is a child of the sell itself instead of the content area so the borders of the button don't
			// do weird visual tricks with the borders of the table section
			cell.Add(button);
			
			return cell;
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing && button != null)
			{
				button.RemoveFromSuperview();
				button.Dispose();
				button = null;
			}
		}
		
		public override string Summary ()
		{
			return button.ToString();
		}
	}
}
