using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class StyledMultilineElement : StyledStringElement, IElementSizing 
    {
        public StyledMultilineElement (string caption) : base (caption) {}
        public StyledMultilineElement (string caption, string value) : base (caption, value) {}
        public StyledMultilineElement (string caption, NSAction tapped) : base (caption, tapped) {}
        public StyledMultilineElement (string caption, string value, UITableViewCellStyle style) : base (caption, value) 
        { 
            this.Style = style;
        }

        public virtual float GetHeight (UITableView tableView, NSIndexPath indexPath)
        {
            SizeF maxSize = new SizeF (tableView.Bounds.Width-40, float.MaxValue);
			
            if (this.Accessory != UITableViewCellAccessory.None)
                maxSize.Width -= 20;
			
            var captionFont = Font ?? UIFont.BoldSystemFontOfSize (17);
            float height = tableView.StringSize (Caption, captionFont, maxSize, LineBreakMode).Height;
			
            if ((this.Style == UITableViewCellStyle.Subtitle) && !String.IsNullOrEmpty (Value)) {
                var subtitleFont = SubtitleFont ?? UIFont.SystemFontOfSize (14);
                height += tableView.StringSize (Value, subtitleFont, maxSize, LineBreakMode).Height;
            }
			
            return height + 10;
        }
    }
}