// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.iOS.Platform;
using MvvmCross.Platform.iOS.Views;
using Foundation;
using MessageUI;
using UIKit;

namespace MvvmCross.Plugins.Email.iOS
{
    public class MvxComposeEmailTask
        : MvxIosTask
        , IMvxComposeEmailTaskEx
    {
        private readonly IMvxIosModalHost _modalHost;
        private MFMailComposeViewController _mail;

        public MvxComposeEmailTask()
        {
            _modalHost = Mvx.Resolve<IMvxIosModalHost>();
        }

        public void ComposeEmail(string to, string cc = null, string subject = null, string body = null,
            bool isHtml = false, string dialogTitle = null)
        {
            var toArray = to == null ? null : new[] { to };
            var ccArray = cc == null ? null : new[] { cc };
            ComposeEmail(
                toArray,
                ccArray,
                subject,
                body,
                isHtml);
        }

        public void ComposeEmail(
            IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null,
            string body = null, bool isHtml = false,
            IEnumerable<EmailAttachment> attachments = null, string dialogTitle = null)
        {
            if (!MFMailComposeViewController.CanSendMail)
                throw new MvxException("This device cannot send mail");

            _mail = new MFMailComposeViewController();
            _mail.SetMessageBody(body ?? string.Empty, isHtml);
            _mail.SetSubject(subject ?? string.Empty);

            if (cc != null)
                _mail.SetCcRecipients(cc.ToArray());

            _mail.SetToRecipients(to?.ToArray() ?? new[] { string.Empty });
            if (attachments != null)
            {
                foreach (var a in attachments)
                {
                    _mail.AddAttachmentData(NSData.FromStream(a.Content), a.ContentType, a.FileName);
                }
            }
            _mail.Finished += HandleMailFinished;

            _modalHost.PresentModalViewController(_mail, true);
        }

        public bool CanSendEmail => MFMailComposeViewController.CanSendMail;

        public bool CanSendAttachments => CanSendEmail;

        private void HandleMailFinished(object sender, MFComposeResultEventArgs e)
        {
            var uiViewController = sender as UIViewController;
            if (uiViewController == null)
            {
                throw new ArgumentException("sender");
            }

            uiViewController.DismissViewController(true, () => { });
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }
    }
}