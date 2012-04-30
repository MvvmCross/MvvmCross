using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Reactive;

namespace Phone7.Fx.Controls
{
    public class CachedImage : Control
    {
        public static readonly DependencyProperty SourceProperty =
           DependencyProperty.Register(
           "Source",
           typeof(ImageSource),
           typeof(CachedImage),
           new PropertyMetadata(OnSourcePropertyChanged));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }



        private static void OnSourcePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CachedImage ctrl = (CachedImage)sender;
            ctrl.OnSourcePropertyChanged(e.OldValue, e.NewValue);
        }


        private Image _image;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedImage"/> class.
        /// </summary>
        public CachedImage()
        {
            this.DefaultStyleKey = typeof(CachedImage);
        }


        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = this.FindVisualChild("LayoutRoot") as Image;

            if (_image == null)
                throw new NotImplementedException("LayoutRoot is required to display the image.");
        }

        /// <summary>
        /// Called when the Source property changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public virtual void OnSourcePropertyChanged(object oldValue, object newValue)
        {

            WebRequest request = WebRequest.Create(((BitmapImage)newValue).UriSource);
            // download the picture in a background thread via Rx
            Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                 .ObserveOnDispatcher()
                 .Subscribe(webResponse =>
                 {
                     try
                     {
                         var bi = new BitmapImage();
                         bi.SetSource(webResponse.GetResponseStream());
                         _image.Source = bi;
                     }
                     catch (Exception)
                     {
                     }
                 });
        }


    }
}