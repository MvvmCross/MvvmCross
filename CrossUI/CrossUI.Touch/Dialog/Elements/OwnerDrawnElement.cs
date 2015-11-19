// OwnerDrawnElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public abstract class OwnerDrawnElement : Element, IElementSizing
    {
        public string CellReuseIdentifier { get; set; }

        public UITableViewCellStyle Style { get; set; }

        protected OwnerDrawnElement(UITableViewCellStyle style, string cellIdentifier)
            : base(null)
        {
            this.CellReuseIdentifier = cellIdentifier;
            this.Style = style;
        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return Height(tableView.Bounds);
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(this.CellReuseIdentifier) as OwnerDrawnCell;

            if (cell == null)
            {
                cell = new OwnerDrawnCell(this, this.Style, this.CellReuseIdentifier);
            }
            else
            {
                cell.Element = this;
            }

            cell.Update();
            return cell;
        }

        public abstract void Draw(CGRect bounds, CGContext context, UIView view);

        public abstract float Height(CGRect bounds);

        private class OwnerDrawnCell : UITableViewCell
        {
            private OwnerDrawnCellView _view;

            public OwnerDrawnCell(OwnerDrawnElement element, UITableViewCellStyle style, string cellReuseIdentifier)
                : base(style, cellReuseIdentifier)
            {
                Element = element;
            }

            public OwnerDrawnElement Element
            {
                get { return _view.Element; }
                set
                {
                    if (_view == null)
                    {
                        _view = new OwnerDrawnCellView(value);
                        ContentView.Add(_view);
                    }
                    else
                    {
                        _view.Element = value;
                    }
                }
            }

            public void Update()
            {
                SetNeedsDisplay();
                _view.SetNeedsDisplay();
            }

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                _view.Frame = ContentView.Bounds;
            }
        }

        private class OwnerDrawnCellView : UIView
        {
            private OwnerDrawnElement _element;

            public OwnerDrawnCellView(OwnerDrawnElement element)
            {
                this._element = element;
            }

            public OwnerDrawnElement Element
            {
                get { return _element; }
                set { _element = value; }
            }

            public void Update()
            {
                SetNeedsDisplay();
            }

            public override void Draw(CGRect rect)
            {
                CGContext context = UIGraphics.GetCurrentContext();
                _element.Draw(rect, context, this);
            }
        }
    }
}