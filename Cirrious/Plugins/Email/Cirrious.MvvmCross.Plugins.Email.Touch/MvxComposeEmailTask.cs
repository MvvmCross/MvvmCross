// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform.Tasks;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.Email.Touch
{
    public class MvxComposeEmailTask 
		: MvxTouchTask
		, IMvxComposeEmailTask
		, IMvxServiceConsumer
    {
        private readonly IMvxTouchViewPresenter _presenter;
        private MFMailComposeViewController _mail;

        public MvxComposeEmailTask()
        {
            _presenter = this.GetService<IMvxTouchViewPresenter>();
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

            _presenter.PresentModalViewController(_mail, true);
        }

        private void HandleMailFinished(object sender, MFComposeResultEventArgs e)
        {
            var uiViewController = sender as UIViewController;
            if (uiViewController == null)
            {
                throw new ArgumentException("sender");
            }

            uiViewController.DismissViewController(true, () => { });
            _presenter.NativeModalViewControllerDisappearedOnItsOwn();
        }
    }
}