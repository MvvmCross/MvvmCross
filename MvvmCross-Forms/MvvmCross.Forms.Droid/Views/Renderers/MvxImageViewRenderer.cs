using System.ComponentModel;
using Android.Widget;
using Mvvmcross.Forms.Droid.Views;
using Mvvmcross.Forms.Views;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using DroidMvxImageView = MvvmCross.Binding.Droid.Views.MvxImageView;

[assembly: ExportRenderer(typeof(MvxImageView), typeof(MvxImageViewRenderer))]
namespace Mvvmcross.Forms.Droid.Views
{
    class MvxImageViewRenderer : ImageRenderer
    {
        private DroidMvxImageView _NativeControl { get; set; }
        private MvxImageView _SharedControl => Element as MvxImageView;

        protected override ImageView CreateNativeControl()
        {
            _NativeControl = new DroidMvxImageView(Context);

            return _NativeControl;
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
            if (args.PropertyName == nameof(MvxImageView.Source))
            {
                MvxTrace.Warning("Source property ignored on MvxImageView");
            }
            else
            {
                base.OnElementPropertyChanged(sender, args);

                if (_NativeControl != null)
                {
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
