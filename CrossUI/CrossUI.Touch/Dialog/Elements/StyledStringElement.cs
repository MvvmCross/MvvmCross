// StyledStringElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using CrossUI.Touch.Dialog.Utilities;
using Foundation;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    /// <summary>
    ///   A version of the StringElement that can be styled with a number of formatting 
    ///   options and can render images or background images either from UIImage parameters 
    ///   or by downloading them from the net.
    /// </summary>
    public class StyledStringElement : StringElement, IImageUpdated, IColorizeBackground
    {
        private static readonly NSString[] skey =
            {
                new NSString(".1"), new NSString(".2"), new NSString(".3"),
                new NSString(".4")
            };

        public StyledStringElement(string caption = "") : base(caption)
        {
        }

        public StyledStringElement(string caption, Action tapped) : base(caption, tapped)
        {
        }

        public StyledStringElement(string caption, string value) : base(caption, value)
        {
            Style = UITableViewCellStyle.Value1;
        }

        public StyledStringElement(string caption, string value, UITableViewCellStyle style) : base(caption, value)
        {
            Style = style;
        }

        private UITableViewCellStyle _style = UITableViewCellStyle.Default;

        protected UITableViewCellStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        public event Action AccessoryTapped;
        public ICommand AccessoryCommand { get; set; }

        public UIFont Font { get; set; }
        public UIFont SubtitleFont { get; set; }
        public UIColor TextColor { get; set; }
        private UILineBreakMode _lineBreakMode = UILineBreakMode.WordWrap;

        public UILineBreakMode LineBreakMode
        {
            get { return _lineBreakMode; }
            set { _lineBreakMode = value; }
        }

        public int Lines { get; set; }
        public UITableViewCellAccessory Accessory { get; set; }

        // To keep the size down for a StyleStringElement, we put all the image information
        // on a separate structure, and create this on demand.
        private ExtraInfo _extraInfo;

        private class ExtraInfo
        {
            public UIImage Image { get; set; } // Maybe add BackgroundImage?
            public UIColor BackgroundColor { get; set; }
            public UIColor DetailColor { get; set; }
            public Uri Uri { get; set; }
            public Uri BackgroundUri { get; set; }
        }

        private ExtraInfo OnImageInfo()
        {
            return _extraInfo ?? (_extraInfo = new ExtraInfo());
        }

        // Uses the specified image (use this or ImageUri)
        public UIImage Image
        {
            get { return _extraInfo?.Image; }
            set
            {
                OnImageInfo().Image = value;
                _extraInfo.Uri = null;
            }
        }

        // Loads the image from the specified uri (use this or Image)
        public Uri ImageUri
        {
            get { return _extraInfo?.Uri; }
            set
            {
                OnImageInfo().Uri = value;
                _extraInfo.Image = null;
            }
        }

        // Background color for the cell (alternative: BackgroundUri)
        public UIColor BackgroundColor
        {
            get { return _extraInfo?.BackgroundColor; }
            set
            {
                OnImageInfo().BackgroundColor = value;
                _extraInfo.BackgroundUri = null;
            }
        }

        public UIColor DetailColor
        {
            get { return _extraInfo?.DetailColor; }
            set { OnImageInfo().DetailColor = value; }
        }

        // Uri for a Background image (alternatiev: BackgroundColor)
        public Uri BackgroundUri
        {
            get { return _extraInfo?.BackgroundUri; }
            set
            {
                OnImageInfo().BackgroundUri = value;
                _extraInfo.BackgroundColor = null;
            }
        }

        protected virtual string GetKey(int style)
        {
            return skey[style];
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var key = GetKey((int) Style);
            var cell = tv.DequeueReusableCell(key) ??
                       new UITableViewCell(Style, key) {SelectionStyle = UITableViewCellSelectionStyle.Blue};
            PrepareCell(cell);
            return cell;
        }


        protected override void UpdateCellDisplay(UITableViewCell cell)
        {
            // note that we deliberately do not call the base class here
            PrepareCell(cell);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            // note that we deliberately do not call the base class here
            PrepareCell(cell);
        }

        protected override void UpdateCaptionDisplay(UITableViewCell cell)
        {
            // note that we deliberately do not call the base class here
            PrepareCell(cell);
        }

        protected virtual void PrepareCell(UITableViewCell cell)
        {
            if (cell == null)
                return;

            // Visible is used here because StyledStringElement does not pass the Update*Detail calls down to its base classes
            // see https://github.com/slodge/MvvmCross/issues/403
            cell.Hidden = !Visible;

            cell.Accessory = Accessory;
            var tl = cell.TextLabel;
            tl.Text = Caption;
            tl.TextAlignment = Alignment;
            tl.TextColor = TextColor ?? UIColor.Black;
            tl.Font = Font ?? UIFont.BoldSystemFontOfSize(17);
            tl.LineBreakMode = LineBreakMode;
            tl.Lines = Lines;

            // The check is needed because the cell might have been recycled.
            if (cell.DetailTextLabel != null)
                cell.DetailTextLabel.Text = Value ?? "";

            if (_extraInfo == null)
            {
                ClearBackground(cell);
            }
            else
            {
                var imgView = cell.ImageView;
                UIImage img;

                if (imgView != null)
                {
                    if (_extraInfo.Uri != null)
                        img = ImageLoader.DefaultRequestImage(_extraInfo.Uri, this);
                    else if (_extraInfo.Image != null)
                        img = _extraInfo.Image;
                    else
                        img = null;
                    imgView.Image = img;
                }

                if (cell.DetailTextLabel != null)
                    cell.DetailTextLabel.TextColor = _extraInfo.DetailColor ?? UIColor.Gray;
            }

            if (cell.DetailTextLabel != null)
            {
                cell.DetailTextLabel.Lines = Lines;
                cell.DetailTextLabel.LineBreakMode = LineBreakMode;
                cell.DetailTextLabel.Font = SubtitleFont ?? UIFont.SystemFontOfSize(14);
                cell.DetailTextLabel.TextColor = (_extraInfo?.DetailColor == null)
                                                     ? UIColor.Gray
                                                     : _extraInfo.DetailColor;
            }
        }

        protected virtual void ClearBackground(UITableViewCell cell)
        {
            cell.BackgroundColor = UIColor.White;
            cell.TextLabel.BackgroundColor = UIColor.Clear;
        }

        void IColorizeBackground.WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            if (_extraInfo == null)
            {
                ClearBackground(cell);
                return;
            }

            if (_extraInfo.BackgroundColor != null)
            {
                cell.BackgroundColor = _extraInfo.BackgroundColor;
                cell.TextLabel.BackgroundColor = UIColor.Clear;
            }
            else if (_extraInfo.BackgroundUri != null)
            {
                var img = ImageLoader.DefaultRequestImage(_extraInfo.BackgroundUri, this);
                cell.BackgroundColor = img == null ? UIColor.White : UIColor.FromPatternImage(img);
                cell.TextLabel.BackgroundColor = UIColor.Clear;
            }
            else
                ClearBackground(cell);
        }

        void IImageUpdated.UpdatedImage(Uri uri)
        {
            if (uri == null || _extraInfo == null)
                return;
            var root = GetImmediateRootElement();
            root?.TableView?.ReloadRows(new[] {IndexPath}, UITableViewRowAnimation.None);
        }

        internal void AccessoryTap()
        {
            Action tapped = AccessoryTapped;
            tapped?.Invoke();
            AccessoryCommand?.Execute(null);
        }
    }
}