using System;
using System.ComponentModel;
using MvvmCross.Forms.iOS.Views.Renderers;
using MvvmCross.Forms.Views;
using MvvmCross.Platform.Platform;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using MvxIosImageView = MvvmCross.Binding.iOS.Views.MvxImageView;

[assembly: ExportRenderer(typeof(MvxImageView), typeof(MvxImageViewRenderer))]
namespace MvvmCross.Forms.iOS.Views.Renderers
{
    internal class MvxImageViewRenderer : ImageRenderer
    {
        private MvxIosImageView _nativeControl;
        private MvxImageView SharedControl => Element as MvxImageView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && _nativeControl != null)
            {
                _nativeControl.ImageChanged -= OnSourceImageChanged;
                _nativeControl.Image = null; // Prevent the base renderer from disposing of this image, DownloadCache takes care of it
            }

            base.Dispose(disposing);
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

            if (Control == null && args.NewElement != null)
            {
                _nativeControl = new MvxIosImageView()
                {
                    ContentMode = UIViewContentMode.ScaleAspectFit,
                    ClipsToBounds = true
                };
                _nativeControl.ImageChanged += OnSourceImageChanged;

                SetNativeControl(_nativeControl);
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

        private void OnSourceImageChanged(object sender, EventArgs args)
        {
            (SharedControl as IVisualElementController).InvalidateMeasure(InvalidationTrigger.MeasureChanged);
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
                    if (args.PropertyName == nameof(MvxImageView.Source))
                    {
                        MvxTrace.Warning("Source property ignored on MvxImageView");
                    }
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
    }
}
