// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Linq;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.WindowsPhone.Tasks;
using Microsoft.Phone.Tasks;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Plugins.Email.WindowsPhone
{
    public class MvxComposeEmailTask
        : MvxWindowsPhoneTask
        , IMvxComposeEmailTaskEx
    {
        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            ComposeEmail(
                new[] { to },
                new[] { cc },
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
            if (attachments != null && attachments.Any())
                throw new MvxException("Don't know how to send attachments in WP");

            var task = new EmailComposeTask { Subject = subject, Body = body };
            if (to != null)
                task.To = string.Join(";", to);
            if (cc != null)
                task.Cc = string.Join(";", cc);

            DoWithInvalidOperationProtection(task.Show);
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
