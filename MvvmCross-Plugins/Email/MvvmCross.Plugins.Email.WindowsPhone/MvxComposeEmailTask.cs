// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.WindowsPhone.Tasks;
using Microsoft.Phone.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Plugins.Email.WindowsPhone
{
    public class MvxComposeEmailTask
        : MvxWindowsPhoneTask
        , IMvxComposeEmailTaskEx
    {
        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null,
            bool isHtml = false, string dialogTitle = null)
        {
            ComposeEmail(
                new[] { to },
                new[] { cc },
                subject,
                body,
                isHtml);
        }

        public void ComposeEmail(
            IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null,
            string body = null, bool isHtml = false,
            IEnumerable<EmailAttachment> attachments = null, string dialogTitle = null)
        {
            if (attachments != null && attachments.Any())
                throw new MvxException("Don't know how to send attachments in WP");

            var task = new EmailComposeTask { Subject = subject, Body = body };
            if (to != null)
                task.To = string.Join(";", to);
            if (cc != null)
                task.Cc = string.Join(";", cc);

            DoWithInvalidOperationProtection(task.Show);
        }

        public bool CanSendEmail => true;

        public bool CanSendAttachments => false;
    }
}