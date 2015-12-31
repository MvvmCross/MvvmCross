// ActivityElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using System;
using UIKit;

namespace CrossUI.iOS.Dialog.Elements
{
    public class ActivityElement : UIViewElement, IElementSizing
    {
        public ActivityElement()
            : base("", new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray), false)
        {
            var sbounds = UIScreen.MainScreen.Bounds;
            var uia = View as UIActivityIndicatorView;

            uia?.StartAnimating();

            var vbounds = View.Bounds;
            View.Frame = new CGRect((sbounds.Width - vbounds.Width) / 2, 4, vbounds.Width, vbounds.Height + 0);
            View.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
        }

        public bool Animating
        {
            get { return ((UIActivityIndicatorView)View).IsAnimating; }
            set
            {
                var activity = View as UIActivityIndicatorView;
                if (value)
                    activity?.StartAnimating();
                else
                    activity?.StopAnimating();
            }
        }

        nfloat IElementSizing.GetHeight(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return base.GetHeight(tableView, indexPath) + 8;
        }
    }
}