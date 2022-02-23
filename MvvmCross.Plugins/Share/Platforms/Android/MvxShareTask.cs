// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using MvvmCross.Platforms.Android;

namespace MvvmCross.Plugin.Share.Platforms.Android
{
    [Preserve(AllMembers = true)]
    public class MvxShareTask
        : MvxAndroidTask, IMvxShareTask
    {
        public void ShareShort(string message)
        {
            var shareIntent = new Intent(Intent.ActionSend);
            shareIntent.PutExtra(Intent.ExtraText, message ?? string.Empty);
            shareIntent.SetType("text/plain");
            StartActivity(shareIntent);
        }

        public void ShareLink(string title, string message, string link)
        {
            var shareIntent = new Intent(Intent.ActionSend);

            shareIntent.PutExtra(Intent.ExtraSubject, title ?? string.Empty);
            shareIntent.PutExtra(Intent.ExtraText, message + " " + link);
            shareIntent.SetType("text/plain");

            StartActivity(shareIntent);
        }
    }
}
