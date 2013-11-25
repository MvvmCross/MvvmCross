// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Text;
using Cirrious.CrossCore.Droid.Platform;

namespace Cirrious.MvvmCross.Plugins.Email.Droid
{
    public class MvxComposeEmailTask
        : MvxAndroidTask
          , IMvxComposeEmailTask
    {
        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            var emailIntent = new Intent(global::Android.Content.Intent.ActionSend);

            // TODO - should we split 'to' and 'cc' based on ';' delimiters
            if (!string.IsNullOrEmpty(to))
                emailIntent.PutExtra(global::Android.Content.Intent.ExtraEmail, new[] { to });
            if (!string.IsNullOrEmpty(cc))
                emailIntent.PutExtra(global::Android.Content.Intent.ExtraCc, new[] { cc });

            emailIntent.PutExtra(global::Android.Content.Intent.ExtraSubject, subject ?? string.Empty);

            if (isHtml) {
                emailIntent.SetType ("text/html");
                emailIntent.PutExtra (global::Android.Content.Intent.ExtraText,
                    string.IsNullOrEmpty (body) ? Html.FromHtml (string.Empty) : Html.FromHtml (body));
            } else {
                emailIntent.SetType ("text/plain");
                emailIntent.PutExtra (global::Android.Content.Intent.ExtraText, body ?? string.Empty);
            }

            StartActivity(emailIntent);
        }
    }
}