// MvxShareTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Cirrious.CrossCore.Droid.Platform;

namespace MvvmCross.Plugins.Share.Droid
{
    public class MvxShareTask
        : MvxAndroidTask
          , IMvxShareTask
    {
        public void ShareShort(string message)
        {
            var shareIntent = new Intent(global::Android.Content.Intent.ActionSend);
            shareIntent.PutExtra(global::Android.Content.Intent.ExtraText, message ?? string.Empty);
            shareIntent.SetType("text/plain");
            StartActivity(shareIntent);
        }

        public void ShareLink(string title, string message, string link)
        {
            var shareIntent = new Intent(global::Android.Content.Intent.ActionSend);

            shareIntent.PutExtra(global::Android.Content.Intent.ExtraSubject, title ?? string.Empty);
            shareIntent.PutExtra(global::Android.Content.Intent.ExtraText, message + " " + link);
            shareIntent.SetType("text/plain");

            StartActivity(shareIntent);
        }
    }
}