// BaseBooleanImageElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using Foundation;
using System;
using System.Windows.Input;
using UIKit;

namespace CrossUI.iOS.Dialog.Elements
{
    public abstract class BaseBooleanImageElement
        : ValueElement<bool>
    {
        private static readonly NSString Key = new NSString("BooleanImageElement");

        #region Cell

        public sealed class TextWithImageCellView : UITableViewCell
        {
            private const int FontSize = 17;
            private static readonly UIFont Font = UIFont.BoldSystemFontOfSize(FontSize);

            private BaseBooleanImageElement _parent;
            private readonly UILabel _label;
            private readonly UIButton _button;

            private const int ImageSpace = 32;
            private const int Padding = 8;

            public TextWithImageCellView(BaseBooleanImageElement parent)
                : base(UITableViewCellStyle.Value1, parent.CellKey)
            {
                this._parent = parent;

                _label = new UILabel
                {
                    TextAlignment = UITextAlignment.Left,
                    Text = parent.Caption,
                    Font = Font,
                    BackgroundColor = UIColor.Clear
                };
                _button = UIButton.FromType(UIButtonType.Custom);
                _button.TouchUpInside += delegate
                    {
                        parent.Value = !parent.Value;
                        UpdateImage();
                        parent.OnButtonTapped();
                    };

                ContentView.Add(_label);
                ContentView.Add(_button);
                UpdateImage();
            }

            private void UpdateImage()
            {
                _button.SetImage(_parent.GetImage(), UIControlState.Normal);
            }

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();
                var full = ContentView.Bounds;
                var frame = full;
                frame.Height = 22;
                frame.X = Padding;
                frame.Y = (full.Height - frame.Height) / 2;
                frame.Width -= ImageSpace + Padding;
                _label.Frame = frame;

                _button.Frame = new CGRect(full.Width - ImageSpace, -3, ImageSpace, 48);
            }

            public void UpdateFrom(BaseBooleanImageElement newParent)
            {
                _parent = newParent;
                UpdateImage();
                _label.Text = _parent.Caption;
                SetNeedsDisplay();
            }
        }

        #endregion Cell

        protected BaseBooleanImageElement(string caption, bool value)
            : base(caption, value)
        {
        }

        public event Action ButtonTapped;

        public ICommand ButtonCommand { get; set; }

        protected virtual void OnButtonTapped()
        {
            ButtonTapped?.Invoke();

            ButtonCommand?.Execute(null);
        }

        protected abstract UIImage GetImage();

        protected override NSString CellKey => Key;

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey) as TextWithImageCellView;
            if (cell == null)
                cell = new TextWithImageCellView(this);
            else
                cell.UpdateFrom(this);
            return cell;
        }

        protected override void UpdateCaptionDisplay(UITableViewCell cell)
        {
            var currentCell = cell as TextWithImageCellView;
            currentCell?.UpdateFrom(this);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            var currentCell = cell as TextWithImageCellView;
            currentCell?.UpdateFrom(this);
        }
    }
}