// MvxImageViewLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Views;
using Foundation;
using System;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxImageViewLoader
        : MvxBaseImageViewLoader<UIImage>
    {
        public MvxImageViewLoader(Func<UIImageView> imageViewAccess, Action afterImageChangeAction = null)
            : base(image =>
            {
                OnUiThread(() => OnImage(imageViewAccess(), image));
                if (afterImageChangeAction != null)
                    OnUiThread(afterImageChangeAction);
            })
        {
        }

        private static void OnImage(UIImageView imageView, UIImage image)
        {
            if (imageView == null) return;
            imageView.Image = image;
        }

        private static void OnUiThread(Action action)
        {
            new NSObject().InvokeOnMainThread(action);
        }
    }
}