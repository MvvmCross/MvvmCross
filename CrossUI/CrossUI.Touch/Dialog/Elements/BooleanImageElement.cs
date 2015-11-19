// BooleanImageElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class BooleanImageElement : BaseBooleanImageElement
    {
        private UIImage _onImage;
        private UIImage _offImage;

        public BooleanImageElement(string caption, bool value, UIImage onImage, UIImage offImage) : base(caption, value)
        {
            this._onImage = onImage;
            this._offImage = offImage;
        }

        protected override UIImage GetImage()
        {
            if (Value)
                return _onImage;
            else
                return _offImage;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _onImage = _offImage = null;
        }
    }
}