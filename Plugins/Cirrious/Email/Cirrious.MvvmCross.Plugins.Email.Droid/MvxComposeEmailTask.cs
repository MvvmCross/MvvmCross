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
using Cirrious.MvvmCross.Plugins.Email;
using System.Collections.Generic;
using System.Linq;

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

            var attachmentList = attachments as IList<EmailAttachment> ?? attachments.ToList();
            if (attachmentList.Any())
            {
                var uris = new List<IParcelable>();

                DoOnActivity(activity =>
                {
                    foreach (var file in attachmentList)
                    {
                        var fileWorking = file;
                        var localFileStream = activity.OpenFileOutput(fileWorking.FileName, FileCreationMode.WorldReadable);
                        var localfile = activity.GetFileStreamPath(fileWorking.FileName);
                        fileWorking.Content.CopyTo(localFileStream);
                        localFileStream.Close();
                        localfile.SetReadable(true, false);
                        uris.Add(Uri.FromFile(localfile));
                        localfile.DeleteOnExit(); // Schedule to delete file when VM quits.
                    }
                });

                emailIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
            }

            // fix for GMail App 5.x (File not found / permission denied when using "StartActivity")
            StartActivityForResult(0, Intent.CreateChooser(emailIntent, string.Empty));
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
