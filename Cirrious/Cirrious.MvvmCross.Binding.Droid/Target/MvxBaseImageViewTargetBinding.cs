// MvxBaseImageViewTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public abstract class MvxBaseImageViewTargetBinding
        : MvxAndroidTargetBinding
    {
        protected ImageView ImageView
        {
            get { return (ImageView) Target; }
        }

        protected MvxBaseImageViewTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override void SetValue(object value)
        {
            var imageView = ImageView;
            if (imageView == null)
            {
                // weak reference is garbage collected - so just return
                return;
            }

            try
            {
                var assetStream = GetStream(value);
                if (assetStream == null)
                    return;

                var options = new BitmapFactory.Options {InPurgeable = true};
                var bitmap = BitmapFactory.DecodeStream(assetStream, null, options);
                var drawable = new BitmapDrawable(Resources.System, bitmap);
                imageView.SetImageDrawable(drawable);
            }
            catch (Exception ex)
            {
                MvxTrace.Error(ex.ToLongString());
                throw;
            }
        }

        protected abstract Stream GetStream(object value);
    }
}