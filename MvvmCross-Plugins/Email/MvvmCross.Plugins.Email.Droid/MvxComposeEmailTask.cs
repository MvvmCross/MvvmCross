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
using MvvmCross.Platform.Droid.Platform;
using System.Collections.Generic;
using System.Linq;
using Java.IO;
using System.IO;
using MvvmCross.Platform.Droid.Views;

namespace MvvmCross.Plugins.Email.Droid
{
    [Preserve(AllMembers = true)]
	public class MvxComposeEmailTask
        : MvxAndroidTask
        , IMvxComposeEmailTaskEx
    {
		private List<Java.IO.File> filesToDelete;

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
                    filesToDelete = new List<Java.IO.File>();

					foreach (var file in attachments)
					{
						// fix for Gmail error
						using (var memoryStream = new MemoryStream())
						{
							var fileName = Path.GetFileNameWithoutExtension(file.FileName);
							var extension = Path.GetExtension(file.FileName);

							// save file in external cache (required so Gmail app can independently access it, otherwise Gmail won't take the attachment)
							var newFile = new Java.IO.File(activity.ExternalCacheDir, fileName + extension);

							file.Content.CopyTo(memoryStream);
							var bytes = memoryStream.ToArray();
							using (var localFileStream = new FileOutputStream(newFile))
							{
								localFileStream.Write(bytes);
							}

							newFile.SetReadable(true, false);
							newFile.DeleteOnExit();
							uris.Add(Uri.FromFile(newFile));

							filesToDelete.Add(newFile);
						}
					}
                });

                if (uris.Any())
                    emailIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
            }

			emailIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
			emailIntent.AddFlags(ActivityFlags.GrantWriteUriPermission);

            // fix for GMail App 5.x (File not found / permission denied when using "StartActivity")
            StartActivityForResult(0, Intent.CreateChooser(emailIntent, dialogTitle ?? string.Empty));
        }

        public bool CanSendEmail => true;

        public bool CanSendAttachments => true;

		protected override void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
		{
			base.ProcessMvxIntentResult(result);

			// on return, delete all attachments from external cache
			foreach (Java.IO.File file in filesToDelete)
			{
				if (file.Exists())
				{
					file.Delete();
				}
			}
			filesToDelete.Clear();
		}
    }
}
