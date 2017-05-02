using System.ComponentModel;
using Mvvmcross.Forms.iOS.Views.Renderers;
using Mvvmcross.Forms.Views;
using MvvmCross.Platform.Platform;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using IosMvxImageView = MvvmCross.Binding.iOS.Views.MvxImageView;

[assembly: ExportRenderer(typeof(MvxImageView), typeof(MvxImageViewRenderer))]
namespace Mvvmcross.Forms.iOS.Views.Renderers
{
    class MvxImageViewRenderer : ImageRenderer
    {
        private IosMvxImageView _NativeControl { get; set; }
        private MvxImageView _SharedControl => Element as MvxImageView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && _NativeControl != null)
            {
                _NativeControl.Image = null; // Prevent the base renderer from disposing of this image, DownloadCache takes care of it
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> args)
        {
            if (args.OldElement != null)
            {
                if (_NativeControl != null)
                {
                    _NativeControl.Dispose();
                    _NativeControl = null;
                }
            }

            if (Control == null && args.NewElement != null)
            {
                _NativeControl = new IosMvxImageView();
                _NativeControl.ContentMode = UIViewContentMode.ScaleAspectFit;
                _NativeControl.ClipsToBounds = true;

                SetNativeControl(_NativeControl);
            }

            if (Element.Source != null)
            {
                MvxTrace.Warning("Source property ignored on MvxImageView");
            }

            base.OnElementChanged(args);

            if (_NativeControl != null)
            {
                if (_NativeControl.ErrorImagePath != _SharedControl.ErrorImagePath)
                {
                    _NativeControl.ErrorImagePath = _SharedControl.ErrorImagePath;
                }

                if (_NativeControl.DefaultImagePath != _SharedControl.DefaultImagePath)
                {
                    _NativeControl.DefaultImagePath = _SharedControl.DefaultImagePath;
                }

                if (_NativeControl.ImageUrl != _SharedControl.ImageUri)
                {
                    _NativeControl.ImageUrl = _SharedControl.ImageUri;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName == nameof(MvxImageView.Source))
            {
                MvxTrace.Warning("Source property ignored on MvxImageView");
            }
            else
            {
                base.OnElementPropertyChanged(sender, args);

                if (_NativeControl != null)
                {
                    if (args.PropertyName == nameof(MvxImageView.Source))
                    {
                        MvxTrace.Warning("Source property ignored on MvxImageView");
                    }
                    if (args.PropertyName == nameof(MvxImageView.DefaultImagePath))
                    {
                        _NativeControl.DefaultImagePath = _SharedControl.DefaultImagePath;
                    }
                    else if (args.PropertyName == nameof(MvxImageView.ErrorImagePath))
                    {
                        _NativeControl.ErrorImagePath = _SharedControl.ErrorImagePath;
                    }
                    else if (args.PropertyName == nameof(MvxImageView.ImageUri))
                    {
                        _NativeControl.ImageUrl = _SharedControl.ImageUri;
                    }
                }
            }
        }
    }
}
