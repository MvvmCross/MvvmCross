#region Copyright

// <copyright file="MultilineElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class MultilineElement : StringElement, IElementSizing
    {
        public MultilineElement(string caption = "") : base(caption)
        {
        }

        public MultilineElement(string caption, string value) : base(caption, value)
        {
        }

        public MultilineElement(string caption, NSAction tapped) : base(caption, tapped)
        {
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = base.GetCellImpl(tv);
            var tl = cell.TextLabel;
            tl.LineBreakMode = UILineBreakMode.WordWrap;
            tl.Lines = 0;

            return cell;
        }

        public virtual float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            var size = new SizeF(280, float.MaxValue);
            using (var font = UIFont.FromName("Helvetica", 17f))
                return tableView.StringSize(Caption, font, size, UILineBreakMode.WordWrap).Height + 10;
        }
    }
}