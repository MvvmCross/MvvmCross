﻿// MvxImageViewDrawableNameTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxImageViewDrawableNameTargetBinding
        : MvxImageViewDrawableTargetBinding
    {
        public MvxImageViewDrawableNameTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (ImageView)target;

            if (!(value is string))
            {
                MvxLog.Instance.Warn(
                    "Value '{0}' could not be parsed as a valid string identifier", value);
                imageView.SetImageDrawable(null);
                return;
            }

            var resources = AndroidGlobals.ApplicationContext.Resources;
            var id = resources.GetIdentifier((string)value, "drawable", AndroidGlobals.ApplicationContext.PackageName);
            if (id == 0)
            {
                MvxLog.Instance.Warn(
                    "Value '{0}' was not a known drawable name", value);
                imageView.SetImageDrawable(null);
                return;
            }

            base.SetValueImpl(target, id);
        }
    }
}
