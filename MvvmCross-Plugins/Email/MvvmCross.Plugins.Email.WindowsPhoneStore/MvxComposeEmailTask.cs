using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel.Email;

namespace MvvmCross.Plugins.Email.WindowsPhoneStore
{
    public class MvxComposeEmailTask
        : IMvxComposeEmailTaskEx
    {
        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null, bool isHtml = false,
            string dialogTitle = null)
        {
            ComposeEmail(new List<string> {to}, new List<string> {cc}, subject, body, isHtml, null, dialogTitle);
        }

        public void ComposeEmail(IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null, string body = null, bool isHtml = false,
            IEnumerable<EmailAttachment> attachments = null, string dialogTitle = null)
        {
            var message = new EmailMessage
            {
                Subject = subject,
                Body = body
            };

            foreach (var recipient in to)
            {
                message.To.Add(new EmailRecipient(recipient));
            }

            if (cc != null)
            {
                foreach (var recipient in cc)
                {
                    message.CC.Add(new EmailRecipient(recipient));
                }
            }

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var stream = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromStream(attachment.Content.AsRandomAccessStream());
                    message.Attachments.Add(new Windows.ApplicationModel.Email.EmailAttachment(attachment.FileName, stream));
                }
            }

            EmailManager.ShowComposeNewEmailAsync(message);

        }

        public bool CanSendEmail => true;
        public bool CanSendAttachments => true;
    }
}
