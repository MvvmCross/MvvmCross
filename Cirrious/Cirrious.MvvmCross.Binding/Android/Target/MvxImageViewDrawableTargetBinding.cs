#region Copyright
// <copyright file="MvxImageViewDrawableTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Android.Graphics.Drawables;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

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

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetType
        {
            get { return typeof(string); }
        }

        public override void SetValue(object value)
        {
            if (value == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Null value passed to ImageView binding");
                return;
            }

            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Empty value passed to ImageView binding");
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
    }
}