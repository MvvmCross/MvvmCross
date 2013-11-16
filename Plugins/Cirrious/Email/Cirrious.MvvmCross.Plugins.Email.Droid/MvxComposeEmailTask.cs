// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Android.Content;
using Cirrious.CrossCore.Droid.Platform;
using System.Collections.Generic;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Plugins.Email.Droid
{
    public class MvxComposeEmailTask
        : MvxAndroidTask
        , IMvxComposeEmailTaskEx
    {
        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            var toArray = to == null ? null: new[] { to };
            var ccArray = cc == null ? null : new[] { cc };
            ComposeEmail(
                toArray,
                ccArray,
                subject,
                body,
                isHtml,
                null);
        }

        public void ComposeEmail(
            IEnumerable<string> to, IEnumerable<string> cc, string subject, 
            string body, bool isHtml, 
            IEnumerable<EmailAttachment> attachments)
        {
            if (attachments != null)
                throw new MvxException("Don't know how to send attachments - must use null collection");

            var emailIntent = new Intent(global::Android.Content.Intent.ActionSend);

            if (to != null)
                emailIntent.PutExtra(global::Android.Content.Intent.ExtraEmail, to.ToArray() );
            if (cc != null)
                emailIntent.PutExtra(global::Android.Content.Intent.ExtraCc, cc.ToArray());

            emailIntent.PutExtra(global::Android.Content.Intent.ExtraSubject, subject ?? string.Empty);

            emailIntent.SetType(isHtml ? "text/html" : "text/plain");

            emailIntent.PutExtra(global::Android.Content.Intent.ExtraText, body ?? string.Empty);

            StartActivity(emailIntent);
        }

        public bool CanSendEmail
        {
            get
            {
                return true;
            }
        }

        public bool CanSendAttachments
        {
            get
            {
                return false;
            }
        }
    }
}
