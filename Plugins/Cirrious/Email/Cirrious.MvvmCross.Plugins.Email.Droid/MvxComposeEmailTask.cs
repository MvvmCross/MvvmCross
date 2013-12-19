// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Android.Content;
using Android.Text;
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

            var emailIntent = new Intent(Intent.ActionSend);

            if (to != null)
                emailIntent.PutExtra(Intent.ExtraEmail, to.ToArray() );
            if (cc != null)
                emailIntent.PutExtra(Intent.ExtraCc, cc.ToArray());

            emailIntent.PutExtra(Intent.ExtraSubject, subject ?? string.Empty);

            body = body ?? string.Empty;

            if (isHtml) 
            {
                emailIntent.SetType("text/html");
                emailIntent.PutExtra(Intent.ExtraText, Html.FromHtml(body));
            } 
            else
            {
                emailIntent.SetType("text/plain");
                emailIntent.PutExtra(Intent.ExtraText, body);
            }

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
