// StyledMultilineElement.cs
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
    public class StyledMultilineElement : StyledStringElement, IElementSizing
    {
        public StyledMultilineElement(string caption = "") : base(caption)
        {
        }

        public StyledMultilineElement(string caption, string value) : base(caption, value)
        {
        }

        public StyledMultilineElement(string caption, Action tapped) : base(caption, tapped)
        {
        }

        public StyledMultilineElement(string caption, string value, UITableViewCellStyle style) : base(caption, value)
        {
            this.Style = style;
        }

        public virtual nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            var maxSize = new CGSize(tableView.Bounds.Width - 40, float.MaxValue);

            if (this.Accessory != UITableViewCellAccessory.None)
                maxSize.Width -= 20;

            var captionFont = Font ?? UIFont.BoldSystemFontOfSize(17);
            nfloat height = Caption.StringSize(captionFont, maxSize, LineBreakMode).Height;

            if ((this.Style == UITableViewCellStyle.Subtitle) && !String.IsNullOrEmpty(Value))
            {
                var subtitleFont = SubtitleFont ?? UIFont.SystemFontOfSize(14);
                height += Value.StringSize(subtitleFont, maxSize, LineBreakMode).Height;
            }

            return height + 10;
        }
    }
}