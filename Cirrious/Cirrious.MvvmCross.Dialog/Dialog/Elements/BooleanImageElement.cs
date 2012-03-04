using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class BooleanImageElement : BaseBooleanImageElement 
    {
        UIImage _onImage;
        UIImage _offImage;

        public BooleanImageElement (string caption, bool value, UIImage onImage, UIImage offImage) : base (caption, value)
        {
            this._onImage = onImage;
            this._offImage = offImage;
        }
		
        protected override UIImage GetImage ()
        {
            if (Value)
                return _onImage;
            else
                return _offImage;
        }

        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);
            _onImage = _offImage = null;
        }
    }
}