// StringElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using System;
using UIKit;

namespace CrossUI.iOS.Dialog.Elements
{
    public class StringElement : ValueElement<string>
    {
        private static readonly NSString Skey = new NSString("StringElement");
        private static readonly NSString SkeyValue = new NSString("StringElementValue");

        public StringElement(string caption = "") : base(caption)
        {
        }

        public StringElement(string caption, string value) : base(caption, value)
        {
        }

        public StringElement(string caption, Action tapped) : base(caption, tapped)
        {
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(Value == null ? Skey : SkeyValue) ??
                       new UITableViewCell(Value == null ? UITableViewCellStyle.Default : UITableViewCellStyle.Value1,
                Skey)
                       {
                           SelectionStyle = IsSelectable
                    ? UITableViewCellSelectionStyle.Blue
                    : UITableViewCellSelectionStyle.None
                       };
            cell.Accessory = UITableViewCellAccessory.None;

            return cell;
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            // The check is needed because the cell might have been recycled.
            if (cell?.DetailTextLabel != null)
            {
                cell.DetailTextLabel.Text = Value ?? string.Empty;
                cell.DetailTextLabel.SetNeedsDisplay();
            }
        }

        public override bool Matches(string text)
        {
            return (Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   base.Matches(text);
        }
    }
}