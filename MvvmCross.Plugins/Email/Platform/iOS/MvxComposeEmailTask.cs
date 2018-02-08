﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MessageUI;
using MvvmCross.iOS.Support.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.iOS.Platform;
using MvvmCross.Platform.iOS.Views;
using UIKit;

namespace MvvmCross.Plugins.Email.iOS
{
    [MvvmCross.Platform.Preserve(AllMembers = true)]
	public class MvxComposeEmailTask
        : MvxIosTask
        , IMvxComposeEmailTaskEx
    {
        private MFMailComposeViewController _mail;

        public MvxComposeEmailTask()
        {
        }

        public Task ComposeEmail(string to, string cc = null, string subject = null, string body = null,
            bool isHtml = false, string dialogTitle = null)
        {
            var toArray = to == null ? null : new[] { to };
            var ccArray = cc == null ? null : new[] { cc };
            return ComposeEmail(
                toArray,
                ccArray,
                subject,
                body,
                isHtml);
        }

        public Task ComposeEmail(
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

            UIApplication.SharedApplication.KeyWindow.GetTopModalHostViewController().PresentViewController(_mail, true, null);
            return Task.CompletedTask;
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

            _mail.Finished -= HandleMailFinished;
            uiViewController.DismissViewController(true, () => { });
            _mail = null;
        }
    }
}
