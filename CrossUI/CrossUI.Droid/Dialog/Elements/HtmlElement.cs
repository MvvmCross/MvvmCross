// HtmlElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Net;
using Android.Views;

namespace CrossUI.Droid.Dialog.Elements
{
#warning Not touched this class... need more explanation...

    public class HtmlElement : StringElement
    {
        // public string Value;

        public HtmlElement(string caption = null, string url = null)
            : base(caption, url, "dialog_labelfieldright")
        {
            Url = Uri.Parse(url);
        }

        public HtmlElement(string caption, Uri uri)
            : base(caption, uri.ToString(), "dialog_labelfieldright")
        {
            Url = uri;
        }

        public Uri Url { get; set; }

        private void OpenUrl(Context context)
        {
            var intent = new Intent(context, typeof(HtmlActivity));
            intent.PutExtra("URL", Url.ToString());
            intent.PutExtra("Title", Caption);
            intent.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            var view = base.GetViewImpl(context, parent);
            Click = (o, e) => OpenUrl(context);
            return view;
        }
    }
}