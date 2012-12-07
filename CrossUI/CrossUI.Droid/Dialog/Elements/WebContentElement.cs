﻿using System;
using Android.Content;
using Android.Views;
using Android.Webkit;

namespace CrossUI.Droid.Dialog.Elements
{
    public class WebContentElement : Element
    {
        public WebContentElement(string url)
            : base(string.Empty)
        {
            Url = Android.Net.Uri.Parse(url);
        }

        public WebContentElement(Android.Net.Uri uri)
            : base(string.Empty)
        {
            Url = uri;
        }

        public Android.Net.Uri Url { get; set; }

        public String WebContent { get; set; }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            var webView = new WebView(context);

            string textColor = ";color:#FFFFFF";
            string backgroundColor = "background:#000000;";

            if (Url == null)
            {
                string body = "<html><head><meta name=\"viewport\" content=\"initial-scale=1.0, user-scalable=no\"/></head><body style=\"-webkit-text-size-adjust:none;{0}margin:10px 15px 15px;font-family:helvetica,arial,sans-serif;font-size:16{1}\">{2}</body></html>";
                webView.LoadDataWithBaseURL(string.Empty, string.Format(body, backgroundColor, textColor, WebContent), "text/html", "utf-8", null);
            }
            else
            {
                webView.LoadUrl(Url.ToString());
            }

            webView.SetVerticalScrollbarOverlay(true);
            webView.SetMinimumHeight(10);

            return webView;
        }
    }
}