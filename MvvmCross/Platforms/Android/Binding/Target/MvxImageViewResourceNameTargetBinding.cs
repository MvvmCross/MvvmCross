// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Widget;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxImageViewResourceNameTargetBinding : MvxImageViewDrawableNameTargetBinding
    {
        public MvxImageViewResourceNameTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        protected override void SetImage(ImageView imageView, int id)
        {
            imageView.SetImageResource(id);
        }
    }
}
