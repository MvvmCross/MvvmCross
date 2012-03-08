using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    /// <summary>
    ///  Used to display a slider on the screen.
    /// </summary>
    public class FloatElement : ValueElement<float>
    {
        private bool _showCaption;
        public bool ShowCaption
        {
            get { return _showCaption; }
            set 
            { 
                _showCaption = value;
                UpdateCaptionDisplay(CurrentAttachedCell);
            }
        }

        private float _minValue;
        public float MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                UpdateSlider();
            }
        }

        private float _maxValue;
        public float MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                UpdateSlider();
            }
        }

        static readonly NSString Skey = new NSString ("FloatElement");
        UIImage _left;
        UIImage _right;
        UISlider _slider;
        SizeF _captionSize;
		
        public FloatElement (UIImage left, UIImage right, float value) : base (null)
        {
            _left = left;
            _right = right;
            MinValue = 0;
            MaxValue = 1;
            Value = value;
            _captionSize = new SizeF(0, 0);
        }
		
        protected override NSString CellKey {
            get {
                return Skey;
            }
        }

        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            var cell = tv.DequeueReusableCell (CellKey);
            if (cell == null)
            {
                cell = new UITableViewCell (UITableViewCellStyle.Default, CellKey);
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            } else
                RemoveTag (cell, 1);

            if (_slider == null){
                _slider = new UISlider(GetSliderRectangle()){
                                                               BackgroundColor = UIColor.Clear,
                                                               Continuous = true,
                                                               Tag = 1
                                                           };
                _slider.ValueChanged += delegate {
                     base.OnUserValueChanged(_slider.Value);
                };
            }

            cell.ContentView.AddSubview (_slider);
            return cell;
        }

        private RectangleF GetSliderRectangle()
        {
            return new RectangleF (10f + _captionSize.Width, 12f, 280f - _captionSize.Width, 7f);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            UpdateSlider();
        }

        protected override void UpdateCaptionDisplay(UITableViewCell cell)
        {
            if (cell == null)
                return;

            if (Caption != null && ShowCaption)
            {
                cell.TextLabel.Text = Caption;
                _captionSize = cell.TextLabel.StringSize(Caption, UIFont.FromName(cell.TextLabel.Font.Name, UIFont.LabelFontSize));
                _captionSize.Width += 10; // Spacing
            }
            else
            {
                _captionSize =new SizeF(0,0);
            }

            if (_slider != null)
                _slider.Frame = GetSliderRectangle();
        }

        private void UpdateSlider()
        {
            if (_slider== null)
                return;

            // TODO - should we do some simple Min<=Val<=Max checking here?

            _slider.MinValue = this.MinValue;
            _slider.MaxValue = this.MaxValue;
            _slider.Value = this.Value;
        }

        public override string Summary ()
        {
            return Value.ToString ();
        }

        protected override void Dispose (bool disposing)
        {
            if (disposing){
                if (_slider != null){
                    _slider.Dispose ();
                    _slider = null;
                }
            }
        }		
    }
}