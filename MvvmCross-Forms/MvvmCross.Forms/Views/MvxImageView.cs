using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxImageView : Image
    {
        public static readonly BindableProperty ImageUriProperty = BindableProperty.Create(nameof(ImageUri), typeof(string), typeof(MvxImageView));
        public static readonly BindableProperty DefaultImagePathProperty = BindableProperty.Create(nameof(DefaultImagePath), typeof(string), typeof(MvxImageView));
        public static readonly BindableProperty ErrorImagePathProperty = BindableProperty.Create(nameof(ErrorImagePath), typeof(string), typeof(MvxImageView));

        public string ImageUri
        {
            get { return (string)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }

        public string DefaultImagePath
        {
            get { return (string)GetValue(DefaultImagePathProperty); }
            set { SetValue(DefaultImagePathProperty, value); }
        }

        public string ErrorImagePath
        {
            get { return (string)GetValue(ErrorImagePathProperty); }
            set { SetValue(ErrorImagePathProperty, value); }
        }

        public MvxImageView() : this(null)
        {
        }

        public MvxImageView(Action afterImageChangeAction)
        {
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}