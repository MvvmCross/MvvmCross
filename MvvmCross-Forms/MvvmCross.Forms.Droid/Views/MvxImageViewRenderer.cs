using System;
using System.ComponentModel;
using Android.Widget;
using MvvmCross.Forms.Droid.Views;
using MvvmCross.Forms.Views;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using MvxDroidImageView = MvvmCross.Binding.Droid.Views.MvxImageView;

[assembly: ExportRenderer(typeof(MvxImageView), typeof(MvxImageViewRenderer))]
namespace MvvmCross.Forms.Droid.Views
{
    internal class MvxImageViewRenderer : ImageRenderer
    {
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
                MvxTrace.Warning("Source property ignored on MvxImageView");
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
                MvxTrace.Warning("Source property ignored on MvxImageView");
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
