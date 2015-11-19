// MvxImageViewDrawableNameTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using Android.Widget;
using Cirrious.CrossCore.Platform;
using System;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxImageViewDrawableNameTargetBinding
        : MvxImageViewDrawableTargetBinding
    {
        public MvxImageViewDrawableNameTargetBinding(ImageView imageView) : base(imageView)
        {
        }

        public override Type TargetType => typeof(string);

        protected override bool GetBitmap(object value, out Bitmap bitmap)
        {
            if (!(value is string))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' could not be parsed as a valid string identifier", value);
                bitmap = null;
                return false;
            }

            var resources = AndroidGlobals.ApplicationContext.Resources;
            var id = resources.GetIdentifier((string)value, "drawable", AndroidGlobals.ApplicationContext.PackageName);
            if (id == 0)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' was not a known drawable name", value);
                bitmap = null;
                return false;
            }

            return base.GetBitmap(id, out bitmap);
        }
    }
}