#region Copyright

// <copyright file="MvxShareTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.Content;
using Cirrious.MvvmCross.Droid.Platform.Tasks;

namespace Cirrious.MvvmCross.Plugins.Share.Droid
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