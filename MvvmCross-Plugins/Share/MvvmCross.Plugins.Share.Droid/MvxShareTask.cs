﻿// MvxShareTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using MvvmCross.Platform.Droid.Platform;

namespace MvvmCross.Plugins.Share.Droid
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