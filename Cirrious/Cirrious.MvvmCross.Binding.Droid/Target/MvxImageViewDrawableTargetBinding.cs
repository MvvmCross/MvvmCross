// MvxImageViewDrawableTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Android.Widget;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxImageViewDrawableTargetBinding
        : MvxBaseImageViewTargetBinding
    {
        public MvxImageViewDrawableTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override Type TargetType
        {
            get { return typeof (int); }
        }

        protected override Stream GetStream(object value)
        {
            if (!(value is int))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' could not be parsed as a valid integer identifier", value);
                return null;
            }

            var resources = AndroidGlobals.ApplicationContext.Resources;
            var stream = resources.OpenRawResource((int) value);

            if (stream == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Could not find a drawable with id '{0}'", value);
                return null;
            }

            return stream;
        }
    }
}