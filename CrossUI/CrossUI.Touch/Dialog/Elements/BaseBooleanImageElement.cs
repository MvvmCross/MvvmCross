using System.Drawing;
using System.Windows.Input;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public abstract class BaseBooleanImageElement 
        : ValueElement<bool>
    {
        static readonly NSString Key = new NSString ("BooleanImageElement");

        #region Cell

        public sealed class TextWithImageCellView : UITableViewCell {
            const int FontSize = 17;
            static readonly UIFont Font = UIFont.BoldSystemFontOfSize (FontSize);

            BaseBooleanImageElement _parent;
            readonly UILabel _label;
            readonly UIButton _button;

            const int ImageSpace = 32;
            const int Padding = 8;
	
            public TextWithImageCellView (BaseBooleanImageElement parent) 
                : base (UITableViewCellStyle.Value1, parent.CellKey)
            {
                this._parent = parent;

                _label = new UILabel () {
                                           TextAlignment = UITextAlignment.Left,
                                           Text = parent.Caption,
                                           Font = Font,
                                           BackgroundColor = UIColor.Clear
                                       };
                _button = UIButton.FromType (UIButtonType.Custom);
                _button.TouchDown += delegate {
                                                 parent.Value = !parent.Value;
                                                 UpdateImage ();
                                                 parent.OnButtonTapped();
                };

                ContentView.Add (_label);
                ContentView.Add (_button);
                UpdateImage ();
            }

            void UpdateImage()
            {
                _button.SetImage (_parent.GetImage (), UIControlState.Normal);
            }
			
            public override void LayoutSubviews ()
            {
                base.LayoutSubviews ();
                var full = ContentView.Bounds;
                var frame = full;
                frame.Height = 22;
                frame.X = Padding;
                frame.Y = (full.Height-frame.Height)/2;
                frame.Width -= ImageSpace+Padding;
                _label.Frame = frame;
				
                _button.Frame = new RectangleF (full.Width-ImageSpace, -3, ImageSpace, 48);
            }
			
            public void UpdateFrom (BaseBooleanImageElement newParent)
            {
                _parent = newParent;
                UpdateImage ();
                _label.Text = _parent.Caption;
                SetNeedsDisplay ();
            }
        }

        #endregion

        protected BaseBooleanImageElement (string caption, bool value)
            : base (caption, value)
        {
        }
		
        public event NSAction ButtonTapped;
        public ICommand ButtonCommand { get; set; }
		
        protected virtual void OnButtonTapped()
        {
            if (ButtonTapped != null)
                ButtonTapped();

            if (ButtonCommand != null)
                ButtonCommand.Execute(null);
        }

        protected abstract UIImage GetImage ();
		
        protected override NSString CellKey {
            get {
                return Key;
            }
        }

        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            var cell = tv.DequeueReusableCell (CellKey) as TextWithImageCellView;
            if (cell == null)
                cell = new TextWithImageCellView (this);
            else
                cell.UpdateFrom (this);
            return cell;
        }

        protected override void UpdateCaptionDisplay(UITableViewCell cell)
        {
            var currentCell = cell as TextWithImageCellView;
            if (currentCell != null)
                currentCell.UpdateFrom(this);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            var currentCell = cell as TextWithImageCellView;
            if (currentCell != null)
                currentCell.UpdateFrom(this);
        }
    }
}