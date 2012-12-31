#region Copyright

// <copyright file="BooleanImageElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using MonoTouch.UIKit;

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