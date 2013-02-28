// MvxImageViewDrawableTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Xml;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Droid.Target
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
            get { return typeof(int); }
        }

        public override void SetValue(object value)
        {
            if (value == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Null value passed to ImageView binding");
                return;
            }

            try
            {
                
                var resources = AndroidGlobals.ApplicationContext.Resources;

                
                int id;

                if (value is int)
                {
                    var stream = resources.OpenRawResource((int) value);

                    if (stream == null)
                    {
                        MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Could not find a drawable with id '" + value + "'");
                        return;    
                    }

                    var options = new BitmapFactory.Options() { InPurgeable = true };
                    var bitmap = BitmapFactory.DecodeStream(stream, null, options);
                    var drawable = new BitmapDrawable(Resources.System, bitmap);
                    _imageView.SetImageDrawable(drawable);
                }
                else
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value '" + value + "' could not be parsed as a valid integer identifier");
                }
                
            }
            catch (Exception ex)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
                throw;
            }

        }
    }
}