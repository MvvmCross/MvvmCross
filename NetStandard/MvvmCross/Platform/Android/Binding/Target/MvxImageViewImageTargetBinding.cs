// MvxImageViewImageTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Android.Widget;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxImageViewImageTargetBinding
        : MvxBaseStreamImageViewTargetBinding
    {
        public MvxImageViewImageTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override Type TargetType => typeof(string);

        protected override Stream GetStream(object value)
        {
            if (value == null)
            {
                MvxLog.Instance.Warn("Null value passed to ImageView binding");
                return null;
            }

            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                MvxLog.Instance.Warn("Empty value passed to ImageView binding");
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