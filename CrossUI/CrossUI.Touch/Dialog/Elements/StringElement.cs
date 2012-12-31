#region Copyright

// <copyright file="StringElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
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

        public StringElement(string caption, NSAction tapped) : base(caption, tapped)
        {
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(Value == null ? Skey : SkeyValue);
            if (cell == null)
            {
                cell = new UITableViewCell(Value == null ? UITableViewCellStyle.Default : UITableViewCellStyle.Value1,
                                           Skey);
                cell.SelectionStyle = IsSelectable
                                          ? UITableViewCellSelectionStyle.Blue
                                          : UITableViewCellSelectionStyle.None;
            }
            cell.Accessory = UITableViewCellAccessory.None;

            return cell;
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (cell == null)
                return;

            // The check is needed because the cell might have been recycled.
            if (cell.DetailTextLabel != null)
            {
                cell.DetailTextLabel.Text = Value ?? string.Empty;
                cell.DetailTextLabel.SetNeedsDisplay();
            }
        }


        public override bool Matches(string text)
        {
            return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) ||
                   base.Matches(text);
        }
    }
}