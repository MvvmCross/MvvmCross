// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Touch.Platform;
using Cirrious.CrossCore.Touch.Views;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Plugins.Email.Touch
{
    public class MvxComposeEmailTask
        : MvxTouchTask
          , IMvxComposeEmailTask         
    {
        private readonly IMvxTouchModalHost _modalHost;
        private MFMailComposeViewController _mail;

        public MvxComposeEmailTask()
        {
            _modalHost = Mvx.Resolve<IMvxTouchModalHost>();
        }

        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {
            if (!MFMailComposeViewController.CanSendMail)
                return;

            _mail = new MFMailComposeViewController();
            _mail.SetMessageBody(body ?? string.Empty, isHtml);
            _mail.SetSubject(subject ?? string.Empty);
            _mail.SetCcRecipients(new[] {cc ?? string.Empty});
            _mail.SetToRecipients(new[] {to ?? string.Empty});
            _mail.Finished += HandleMailFinished;

            _modalHost.PresentModalViewController(_mail, true);
        }

		public void ComposeEmail(
			string[] to, string[] cc, string subject, 
			string body, bool isHtml, 
			List<EmailAttachment> attachments)
		{
			_mail = new MFMailComposeViewController();
            _mail.SetMessageBody(body ?? string.Empty, isHtml);
            _mail.SetSubject(subject ?? string.Empty);
            _mail.SetCcRecipients(cc ?? new[] {string.Empty});
            _mail.SetToRecipients(to ?? new[] {string.Empty});
			attachments.ForEach (a => _mail.AddAttachmentData(NSData.FromStream(a.Content), a.ContentType, a.FileName));
			_mail.Finished += HandleMailFinished;

			_modalHost.PresentModalViewController(_mail, true);
		}

		public bool CanSendEmail()
		{
			return MFMailComposeViewController.CanSendMail;
		}

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
