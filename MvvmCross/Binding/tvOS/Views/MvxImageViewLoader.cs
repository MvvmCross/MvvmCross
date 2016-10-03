// MvxImageViewLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views
{
    using System;

    using Foundation;

    using MvvmCross.Binding.Views;

    using UIKit;

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