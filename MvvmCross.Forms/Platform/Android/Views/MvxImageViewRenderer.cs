// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Android.Widget;
using MvvmCross.Base.Logging;
using MvvmCross.Forms.Base;
using MvvmCross.Forms.Platform.Android.Views;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using MvxDroidImageView = MvvmCross.Binding.Droid.Views.MvxImageView;

[assembly: ExportRenderer(typeof(MvxImageView), typeof(MvxImageViewRenderer))]
namespace MvvmCross.Forms.Platform.Android.Views
{
    internal class MvxImageViewRenderer : ImageRenderer
    {
        public MvxImageViewRenderer(Context context) : base(context)
        {
        }

        private MvxDroidImageView _nativeControl;
        private MvxImageView SharedControl => Element as MvxImageView;

        protected override ImageView CreateNativeControl()
        {
            _nativeControl = new MvxDroidImageView(Context);
            _nativeControl.ImageChanged += OnSourceImageChanged;

            return _nativeControl;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> args)
        {
            if (args.OldElement != null)
            {
                if (_nativeControl != null)
                {
                    _nativeControl.ImageChanged -= OnSourceImageChanged;
                    _nativeControl.Dispose();
                    _nativeControl = null;
                }
            }
            
            if (Element.Source != null)
            {
                MvxFormsLog.Instance.Warn("Source property ignored on MvxImageView");
            }

            base.OnElementChanged(args);

            if (_nativeControl != null)
            {
                if (_nativeControl.ErrorImagePath != SharedControl.ErrorImagePath)
                {
                    _nativeControl.ErrorImagePath = SharedControl.ErrorImagePath;
                }

                if (_nativeControl.DefaultImagePath != SharedControl.DefaultImagePath)
                {
                    _nativeControl.DefaultImagePath = SharedControl.DefaultImagePath;
                }

                if (_nativeControl.ImageUrl != SharedControl.ImageUri)
                {
                    _nativeControl.ImageUrl = SharedControl.ImageUri;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(MvxImageView.Source))
            {
                MvxFormsLog.Instance.Warn("Source property ignored on MvxImageView");
            }
            else
            {
                base.OnElementPropertyChanged(sender, args);

                if (_nativeControl != null)
                {
                    if (args.PropertyName == nameof(MvxImageView.DefaultImagePath))
                    {
                        _nativeControl.DefaultImagePath = SharedControl.DefaultImagePath;
                    }
                    else if (args.PropertyName == nameof(MvxImageView.ErrorImagePath))
                    {
                        _nativeControl.ErrorImagePath = SharedControl.ErrorImagePath;
                    }
                    else if (args.PropertyName == nameof(MvxImageView.ImageUri))
                    {
                        _nativeControl.ImageUrl = SharedControl.ImageUri;
                    }
                }
            }
        }

        private void OnSourceImageChanged(object sender, EventArgs args)
        {
            (SharedControl as IVisualElementController).InvalidateMeasure(InvalidationTrigger.MeasureChanged);
        }
    }
}
