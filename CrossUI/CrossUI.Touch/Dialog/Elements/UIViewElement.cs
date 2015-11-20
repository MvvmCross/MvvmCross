// UIViewElement.cs
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
    ///   This element can be used to insert an arbitrary UIView
    /// </summary>
    /// <remarks>
    ///   There is no cell reuse here as we have a 1:1 mapping
    ///   in this case from the UIViewElement to the cell that
    ///   holds our view.
    /// </remarks>
    public class UIViewElement : Element, IElementSizing
    {
        private static int _count;
        private readonly NSString key;
        protected UIView View { get; set; }
        public CellFlags Flags { get; set; }

        public enum CellFlags
        {
            Transparent = 1,
            DisableSelection = 2
        }

        /// <summary>
        ///   Constructor
        /// </summary>
        /// <param name="caption">
        /// The caption, only used for RootElements that might want to summarize results
        /// </param>
        /// <param name="view">
        /// The view to display
        /// </param>
        /// <param name="transparent">
        /// If this is set, then the view is responsible for painting the entire area,
        /// otherwise the default cell paint code will be used.
        /// </param>
        public UIViewElement(string caption, UIView view, bool transparent) : base(caption)
        {
            this.View = view;
            this.Flags = transparent ? CellFlags.Transparent : 0;
            key = new NSString("UIViewElement" + _count++);
        }

        protected override NSString CellKey => key;

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellKey);
                if ((Flags & CellFlags.Transparent) != 0)
                {
                    cell.BackgroundColor = UIColor.Clear;

                    //
                    // This trick is necessary to keep the background clear, otherwise
                    // it gets painted as black
                    //
                    cell.BackgroundView = new UIView(CGRect.Empty)
                    {
                        BackgroundColor = UIColor.Clear
                    };
                }
                if ((Flags & CellFlags.DisableSelection) != 0)
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;

                if (Caption != null)
                    cell.TextLabel.Text = Caption;
                cell.ContentView.AddSubview(View);
            }
            return cell;
        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return View.Bounds.Height;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (View != null)
                {
                    View.Dispose();
                    View = null;
                }
            }
        }
    }
}