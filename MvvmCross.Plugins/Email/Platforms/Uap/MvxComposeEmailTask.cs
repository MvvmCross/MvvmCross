// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Storage;

namespace MvvmCross.Plugin.Email.Platforms.Uap
{
    public class MvxComposeEmailTask
      : IMvxComposeEmailTaskEx
    {
        public bool CanSendAttachments => true;

        public bool CanSendEmail => true;

        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null, bool isHtml = false, string dialogTitle = null)
        {
            var tos = to == null ? null : new[] { to };
            var ccs = cc == null ? null : new[] { cc };
            ComposeEmail(
                tos,
                ccs,
                subject,
                body,
                isHtml,
                null,
                dialogTitle);
        }

        public async void ComposeEmail(IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null, string body = null, bool isHtml = false, IEnumerable<EmailAttachment> attachments = null, string dialogTitle = null)
        {
            //TODO: It is better to have this function as async Task so to avoid exception swallowing
            EmailMessage email = new EmailMessage();

            if (to != null)
            {
                foreach (var item in to)
                {
                    email.To.Add(new EmailRecipient(item));
                }
            }

            if (cc != null)
            {
                foreach (var item in cc)
                {
                    email.CC.Add(new EmailRecipient(item));
                }
            }

            email.Subject = subject ?? "";
            email.Body = body ?? "";

            if (attachments != null)
            {
                foreach (var item in attachments)
                {
                    email.Attachments.Add(
                        new Windows.ApplicationModel.Email.EmailAttachment(
                            item.FileName, await GetTextFile(item)));
                }
            }

            await EmailManager.ShowComposeNewEmailAsync(email);
        }

        private static async Task<StorageFile> GetTextFile(EmailAttachment attachement)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(attachement.FileName, CreationCollisionOption.ReplaceExisting);

            using (var reader = new StreamReader(attachement.Content))
            {
                await FileIO.WriteTextAsync(file, await reader.ReadToEndAsync());
            }
            return file;
        }
    }
}
