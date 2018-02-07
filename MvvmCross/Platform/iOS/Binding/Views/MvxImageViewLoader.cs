// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Foundation;
using MvvmCross.Binding.Views;
using UIKit;

namespace MvvmCross.Platform.Ios.Binding.Views
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
