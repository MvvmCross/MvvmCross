using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace BestSellers.Droid.Controls
{
#warning This control needs to use a cache - e.g. the MonoTouch one
    public class HttpImageView
        : ImageView
    {
        public HttpImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            var typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.HttpImageView);

            int numStyles = typedArray.IndexCount;
            for (var i = 0; i < numStyles; ++i)
            {
                int attributeId = typedArray.GetIndex(i);
                switch (attributeId)
                {
                    case Resource.Styleable.HttpImageView_httpSource:
                        HttpSource = typedArray.GetString(attributeId);
                        break;
                }
            }
            typedArray.Recycle();
        }

        public HttpImageView(Context context)
            : base(context)
        {
        }

        protected HttpImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private string _httpSource;
        public string HttpSource
        {
            get { return _httpSource; }
            set
            {
                if (_httpSource == value)
                    return;
                _httpSource = value;
                HackLoadImage();
            }
        }

        private void HackLoadImage()
        {
            if (string.IsNullOrEmpty(HttpSource))
                return;

            var source = HttpSource;
            var wc = new WebClient();
            wc.DownloadDataCompleted += (sender, args) =>
                                            {
                                                if (args.Error != null)
                                                {
                                                    MvxTrace.Trace("Error inside image download {0}, error {1}", source,
                                                                  args.Error.Message);
                                                    return;
                                                }
                                                try
                                                {
                                                    DecodeAndDisplay(source, args.Result);
                                                }
                                                catch (Exception)
                                                {
                                                    throw;
                                                }
                                            };
            wc.DownloadDataAsync(new Uri(source));
        }

        private void DecodeAndDisplay(string source, byte[] imageData)
        {
            if (source != HttpSource)
                return;

            System.Threading.ThreadPool.QueueUserWorkItem((ignored) => DecodeAndShow(source, imageData));
        }

        private void DecodeAndShow(string source, byte[] imageData)
        {
            try
            {
                var image = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
                
                ((Activity)this.Context).RunOnUiThread(() =>
                {
                    if (source != HttpSource)
                        return;
                    SetImageBitmap(image);
                });
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Exception masked in decoding downloaded image " + exception.ToLongString() );
            }
        }
    }
}