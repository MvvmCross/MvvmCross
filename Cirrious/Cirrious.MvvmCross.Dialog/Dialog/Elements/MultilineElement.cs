using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class MultilineElement : StringElement, IElementSizing {
        public MultilineElement (string caption) : base (caption)
        {
        }
		
        public MultilineElement (string caption, string value) : base (caption, value)
        {
        }
		
        public MultilineElement (string caption, NSAction tapped) : base (caption, tapped)
        {
        }
		
        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            var cell = base.GetCellImpl (tv);				
            var tl = cell.TextLabel;
            tl.LineBreakMode = UILineBreakMode.WordWrap;
            tl.Lines = 0;

            return cell;
        }
		
        public virtual float GetHeight (UITableView tableView, NSIndexPath indexPath)
        {
            SizeF size = new SizeF (280, float.MaxValue);
            using (var font = UIFont.FromName ("Helvetica", 17f))
                return tableView.StringSize (Caption, font, size, UILineBreakMode.WordWrap).Height + 10;
        }
    }
}