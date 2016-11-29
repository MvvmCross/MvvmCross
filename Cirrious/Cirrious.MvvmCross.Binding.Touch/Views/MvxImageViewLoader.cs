// MvxImageViewLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxImageViewLoader
        : MvxBaseImageViewLoader<UIImage>
    {
        public MvxImageViewLoader(Func<UIImageView> imageViewAccess,Action beforeImageChangeAction = null, Action afterImageChangeAction = null)
            : base((image) =>
                {
				if (beforeImageChangeAction != null)
					beforeImageChangeAction();
                    OnImage(imageViewAccess(), image);
				if (afterImageChangeAction != null)
					afterImageChangeAction();
                })
        {
        }

        private static void OnImage(UIImageView imageView, UIImage image)
        {
            if (imageView != null && image != null)
                imageView.Image = image;
        }
    }
}
