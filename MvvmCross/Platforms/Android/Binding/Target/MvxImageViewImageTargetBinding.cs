// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Android.Widget;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxImageViewImageTargetBinding
        : MvxBaseStreamImageViewTargetBinding
    {
        public MvxImageViewImageTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override Type TargetValueType => typeof(string);

        protected override Stream GetStream(object value)
        {
            if (value == null)
            {
                MvxBindingLog.Warning("Null value passed to ImageView binding");
                return null;
            }

            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                MvxBindingLog.Warning("Empty value passed to ImageView binding");
                return null;
            }

            var drawableResourceName = GetImageAssetName(stringValue);
            var assetStream = AndroidGlobals.ApplicationContext.Assets.Open(drawableResourceName);

            return assetStream;
        }

        private static string GetImageAssetName(string rawImage)
        {
            return rawImage.TrimStart('/');
        }
    }
}
