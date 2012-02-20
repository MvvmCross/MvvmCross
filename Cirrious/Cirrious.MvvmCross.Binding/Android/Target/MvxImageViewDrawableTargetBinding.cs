using System;
using Android.Graphics.Drawables;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Android.Target
{
    public class MvxImageViewDrawableTargetBinding 
        : MvxBaseAndroidTargetBinding
    {
        private readonly ImageView _imageView;

        public MvxImageViewDrawableTargetBinding(ImageView imageView)
        {
            _imageView = imageView;
        }

        public override void SetValue(object value)
        {
            if (value == null)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Warning, "Null value passed to ImageView binding");
                return;
            }

            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Warning, "Empty value passed to ImageView binding");
                return;
            }

            var drawableResourceName = GetImageAssetName(stringValue);
            var assetStream = AndroidGlobals.ApplicationContext.Assets.Open(drawableResourceName);
            Drawable drawable = Drawable.CreateFromStream(assetStream, null);
            _imageView.SetImageDrawable(drawable);
        }

        private static string GetImageAssetName(string rawImage)
        {
            return rawImage.TrimStart('/');
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetType
        {
            get { return typeof(string); }
        }
    }
}