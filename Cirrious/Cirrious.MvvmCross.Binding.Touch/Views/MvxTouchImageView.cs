// MvxTouchImageView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Drawing;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    [Obsolete("Use MvxImageView")]
    public class MvxTouchImageView
        : MvxImageView
    {
        #region constructors

        public MvxTouchImageView()
        {
        }

		public MvxTouchImageView(IntPtr handle)
			: base(handle)
		{
		}

        public MvxTouchImageView(RectangleF frame)
            : base(frame)
        {
        }

        #endregion
    }
}