// ElementBadge.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
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
    public class BadgeElement : Element, IElementSizing
    {
        private static readonly NSString ckey = new NSString("badgeKey");
        public UILineBreakMode LineBreakMode { get; set; }
        public UIViewContentMode ContentMode { get; set; }
        public int Lines { get; set; }
        public UITableViewCellAccessory Accessory { get; set; }

        private readonly UIImage _image;
        private UIFont _font;

        public BadgeElement(UIImage badgeImage, string cellText, Action tapped = null)
            : base(cellText)
        {
            LineBreakMode = UILineBreakMode.TailTruncation;
            ContentMode = UIViewContentMode.Left;
            Lines = 1;
            Accessory = UITableViewCellAccessory.None;

            if (badgeImage == null)
                throw new ArgumentNullException(nameof(badgeImage));

            _image = badgeImage;

            if (tapped != null)
                Tapped += tapped;
        }

        public UIFont Font
        {
            get { return _font ?? (_font = UIFont.FromName("Helvetica", 17f)); }
            set
            {
                _font?.Dispose();
                _font = value;
            }
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(ckey) ?? new UITableViewCell(UITableViewCellStyle.Default, ckey)
            {
                SelectionStyle = UITableViewCellSelectionStyle.Blue
            };
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

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            var size = new CGSize(tableView.Bounds.Width - 40, float.MaxValue);
            var height = Caption.StringSize(Font, size, LineBreakMode).Height + 10;

            // Image is 57 pixels tall, add some padding
            return NMath.Max(height, 63);
        }

        public static UIImage MakeCalendarBadge(UIImage template, string smallText, string bigText)
        {
            using (var cs = CGColorSpace.CreateDeviceRGB())
            {
                using (
                    var context = new CGBitmapContext(IntPtr.Zero, 57, 57, 8, 57 * 4, cs,
                                                      CGImageAlphaInfo.PremultipliedLast))
                {
                    //context.ScaleCTM (0.5f, -1);
                    context.TranslateCTM(0, 0);
                    context.DrawImage(new CGRect(0, 0, 57, 57), template.CGImage);
                    context.SetFillColor(1, 1, 1, 1);

                    context.SelectFont("Helvetica", 10f, CGTextEncoding.MacRoman);

                    // Pretty lame way of measuring strings, as documented:
                    var start = context.TextPosition.X;
                    context.SetTextDrawingMode(CGTextDrawingMode.Invisible);
                    context.ShowText(smallText);
                    var width = context.TextPosition.X - start;

                    context.SetTextDrawingMode(CGTextDrawingMode.Fill);
                    context.ShowTextAtPoint((57 - width) / 2, 46, smallText);

                    // The big string
                    context.SelectFont("Helvetica-Bold", 32, CGTextEncoding.MacRoman);
                    start = context.TextPosition.X;
                    context.SetTextDrawingMode(CGTextDrawingMode.Invisible);
                    context.ShowText(bigText);
                    width = context.TextPosition.X - start;

                    context.SetFillColor(0, 0, 0, 1);
                    context.SetTextDrawingMode(CGTextDrawingMode.Fill);
                    context.ShowTextAtPoint((57 - width) / 2, 9, bigText);

                    context.StrokePath();

                    return UIImage.FromImage(context.ToImage());
                }
            }
        }
    }
}