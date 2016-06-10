// MvxAppCompatImageViewImageTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System;
    using System.IO;

    using Android.Support.V7.Widget;

    using MvvmCross.Binding;
    using MvvmCross.Platform.Platform;

    public class MvxAppCompatImageViewImageTargetBinding
        : MvxAppCompatBaseStreamImageViewTargetBinding
    {
        public MvxAppCompatImageViewImageTargetBinding(AppCompatImageView imageView)
            : base(imageView)
        {
        }

        public override Type TargetType => typeof(string);

        protected override Stream GetStream(object value)
        {
            if (value == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Null value passed to AppCompatImageView binding");
                return null;
            }

            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Empty value passed to AppCompatImageView binding");
                return null;
            }

            var drawableResourceName = GetImageAssetName(stringValue);
            var assetStream = this.AndroidGlobals.ApplicationContext.Assets.Open(drawableResourceName);

            return assetStream;
        }

        private static string GetImageAssetName(string rawImage)
        {
            return rawImage.TrimStart('/');
        }
    }
}