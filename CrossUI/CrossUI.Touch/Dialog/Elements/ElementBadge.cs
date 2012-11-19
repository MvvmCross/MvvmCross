//
// ElementBadge.cs: defines the Badge Element.
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//

using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{	
	/// <summary>
	///    This element can be used to show an image with some text
	/// </summary>
	/// <remarks>
	///    The font can be configured after the element has been created
	///    by assignign to the Font property;   If you want to render
	///    multiple lines of text, set the MultiLine property to true.
	/// 
	///    If no font is specified, it will default to Helvetica 17.
	/// 
	///    A static method MakeCalendarBadge is provided that can 
	///    render a calendar badge like the iPhone OS.   It will compose
	///    the text on top of the image which is expected to be 57x57
	/// </remarks>
	public class BadgeElement : Element, IElementSizing {
		static NSString ckey = new NSString ("badgeKey");
        public UILineBreakMode LineBreakMode { get; set; }
        public UIViewContentMode ContentMode { get; set; }
	    public int Lines { get; set; }
	    public UITableViewCellAccessory Accessory { get; set; }

	    readonly UIImage _image;
		UIFont _font;
	
		public BadgeElement (UIImage badgeImage, string cellText)
			: this (badgeImage, cellText, null)
		{
		}

		public BadgeElement (UIImage badgeImage, string cellText, NSAction tapped) 
            : base (cellText)
		{
            LineBreakMode = UILineBreakMode.TailTruncation;
            ContentMode = UIViewContentMode.Left;
            Lines = 1;
            Accessory = UITableViewCellAccessory.None;

            if (badgeImage == null)
				throw new ArgumentNullException ("badgeImage");
			
			_image = badgeImage;

			if (tapped != null)
				Tapped += tapped;
		}		
	
		public UIFont Font {
			get {
				if (_font == null)
					_font = UIFont.FromName ("Helvetica", 17f);
				return _font;
			}
			set {
				if (_font != null)
					_font.Dispose ();
				_font = value;
			}
		}
		
		protected override UITableViewCell GetCellImpl (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (ckey);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, ckey) {
					SelectionStyle = UITableViewCellSelectionStyle.Blue
				};
			}
			cell.Accessory = Accessory;
			var tl = cell.TextLabel;
			tl.Text = Caption;
			tl.Font = Font;
			tl.LineBreakMode = LineBreakMode;
			tl.Lines = Lines;
			tl.ContentMode = ContentMode;
			
			cell.ImageView.Image = _image;
			
			return cell;
		}

		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			var size = new SizeF (tableView.Bounds.Width - 40, float.MaxValue);
			var height = tableView.StringSize (Caption, Font, size, LineBreakMode).Height + 10;
			
			// Image is 57 pixels tall, add some padding
			return Math.Max (height, 63);
		}
		
		public static UIImage MakeCalendarBadge (UIImage template, string smallText, string bigText)
		{
			using (var cs = CGColorSpace.CreateDeviceRGB ()){
				using (var context = new CGBitmapContext (IntPtr.Zero, 57, 57, 8, 57*4, cs, CGImageAlphaInfo.PremultipliedLast)){
					//context.ScaleCTM (0.5f, -1);
					context.TranslateCTM (0, 0);
					context.DrawImage (new RectangleF (0, 0, 57, 57), template.CGImage);
					context.SetRGBFillColor (1, 1, 1, 1);
					
					context.SelectFont ("Helvetica", 10f, CGTextEncoding.MacRoman);
					
					// Pretty lame way of measuring strings, as documented:
					var start = context.TextPosition.X;					
					context.SetTextDrawingMode (CGTextDrawingMode.Invisible);
					context.ShowText (smallText);
					var width = context.TextPosition.X - start;
					
					context.SetTextDrawingMode (CGTextDrawingMode.Fill);
					context.ShowTextAtPoint ((57-width)/2, 46, smallText);
					
					// The big string
					context.SelectFont ("Helvetica-Bold", 32, CGTextEncoding.MacRoman);					
					start = context.TextPosition.X;
					context.SetTextDrawingMode (CGTextDrawingMode.Invisible);
					context.ShowText (bigText);
					width = context.TextPosition.X - start;
					
					context.SetRGBFillColor (0, 0, 0, 1);
					context.SetTextDrawingMode (CGTextDrawingMode.Fill);
					context.ShowTextAtPoint ((57-width)/2, 9, bigText);
					
					context.StrokePath ();
				
					return UIImage.FromImage (context.ToImage ());
				}
			}
		}
	}
}