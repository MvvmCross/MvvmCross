// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Net;
using Android.OS;
using Android.Text;
using Cirrious.CrossCore.Droid.Platform;
using System.Collections.Generic;
using System.Linq;
using Java.IO;

namespace MvvmCross.Plugins.Email.Droid
{
    public class MvxComposeEmailTask
        : MvxAndroidTask
        , IMvxComposeEmailTaskEx
    {
        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null, bool isHtml = false, string dialogTitle = null)
        {
            var toArray = to == null ? null: new[] { to };
            var ccArray = cc == null ? null : new[] { cc };
            ComposeEmail(
                toArray,
                ccArray,
                subject,
                body,
                isHtml,
                null,
                dialogTitle);
        }

        public void ComposeEmail(
            IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null,
            string body = null, bool isHtml = false,
            IEnumerable<EmailAttachment> attachments = null, string dialogTitle = null)
        {
            // http://stackoverflow.com/questions/2264622/android-multiple-email-attachments-using-intent
            var emailIntent = new Intent(Intent.ActionSendMultiple);

            if (to != null)
            {
                emailIntent.PutExtra(Intent.ExtraEmail, to.ToArray());
            }
            if (cc != null)
            {
                emailIntent.PutExtra(Intent.ExtraCc, cc.ToArray());
            }
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

            if (attachments != null)
            {
                var uris = new List<IParcelable>();

                DoOnActivity(activity => {
                    foreach (var file in attachments)
                    {
                        File localfile;
                        using (var localFileStream = activity.OpenFileOutput(
                            file.FileName, FileCreationMode.WorldReadable))
                        {
                            localfile = activity.GetFileStreamPath(file.FileName);
                            file.Content.CopyTo(localFileStream);
                        }
                        localfile.SetReadable(true, false);
                        uris.Add(Uri.FromFile(localfile));
                        localfile.DeleteOnExit(); // Schedule to delete file when VM quits.
                    }
                });

                if (uris.Any())
                    emailIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
            }

            // fix for GMail App 5.x (File not found / permission denied when using "StartActivity")
            StartActivityForResult(0, Intent.CreateChooser(emailIntent, dialogTitle ?? string.Empty));
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
                return true;
            }
        }
    }
}
