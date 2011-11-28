using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Reactive;
using Phone7.Fx.Extensions;

namespace Phone7.Fx.Controls.LoadingImage
{
    public partial class LoadingImage : UserControl
    {
        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register(
          "Source",
          typeof(string),
          typeof(LoadingImage),
          new PropertyMetadata(OnSourcePropertyChanged));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register(
          "Text",
          typeof(string),
          typeof(LoadingImage), new PropertyMetadata(OnTextPropertyChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ErrorMessageProperty =
         DependencyProperty.Register(
         "ErrorMessage",
         typeof(string),
         typeof(LoadingImage), new PropertyMetadata(ErrorMessagePropertyChanged));

        private static void ErrorMessagePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoadingImage ctrl = (LoadingImage)sender;
            ctrl.OnErrorMessagePropertyChanged(e.OldValue, e.NewValue);
        }


        private void OnErrorMessagePropertyChanged(object oldValue, object newValue)
        {
            if (newValue != null)
                LoadingMessage.Text = newValue.ToString();
        }

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        private static void OnTextPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoadingImage ctrl = (LoadingImage)sender;
            ctrl.OnTextPropertyChanged(e.OldValue, e.NewValue);
        }

        private void OnTextPropertyChanged(object oldValue, object newValue)
        {
            //if (newValue != null)
                
        }

        private static void OnSourcePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoadingImage ctrl = (LoadingImage)sender;
            ctrl.OnSourcePropertyChanged(e.OldValue, e.NewValue);
        }

        public virtual void OnSourcePropertyChanged(object oldValue, object newValue)
        {
            progress1.Width = this.Width;
            progress1.IsIndeterminate = true;
            progress1.Visibility = LoadingMessage.Visibility = Visibility.Visible;
            LoadingMessage.Text = this.Text;

            WebRequest request = WebRequest.Create(new Uri(newValue.ToString(), UriKind.Absolute));
            // download the picture in a background thread via Rx
            Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                 .ObserveOn(Scheduler.Dispatcher.AsSafe((e) =>
                                                            {
                                                                LoadingMessage.Text = this.ErrorMessage;
                                                                progress1.IsIndeterminate = false;
                                                                progress1.Visibility = Visibility.Collapsed;
                                                            }))
                 .Subscribe(webResponse =>
                 {
                     try
                     {
                         progress1.IsIndeterminate = false;
                         progress1.Visibility = LoadingMessage.Visibility = Visibility.Collapsed;

                         var bi = new BitmapImage();
                         bi.SetSource(webResponse.GetResponseStream());
                         img.Source = bi;
                     }
                     catch (Exception)
                     {


                     }
                 });
        }

        public LoadingImage()
        {
            InitializeComponent();
        }
    }
}
