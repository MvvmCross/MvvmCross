#region Copyright
// <copyright file="MvxComposeEmailTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;
using Cirrious.MvvmCross.Touch.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Platform.Tasks
{
    public class MvxComposeEmailTask : MvxTouchTask, IMvxComposeEmailTask
    {
        private readonly IMvxTouchViewPresenter _presenter;
        private MFMailComposeViewController _mail;

        public MvxComposeEmailTask (IMvxTouchViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public void ComposeEmail(string to, string cc, string subject, string body, bool isHtml)
        {

            if (!MFMailComposeViewController.CanSendMail)
                return;

            _mail = new MFMailComposeViewController ();
            _mail.SetMessageBody (body ?? string.Empty, isHtml);
            _mail.SetSubject(subject ?? string.Empty);
            _mail.SetCcRecipients(new [] {cc ?? string.Empty});
            _mail.SetToRecipients(new [] {to ?? string.Empty});
            _mail.Finished += HandleMailFinished;
            
            _presenter.PresentModalViewController(_mail, true);
        }

        private void HandleMailFinished(object sender, MFComposeResultEventArgs e)
        {
			(sender as UIViewController).DismissViewController(true, () => {});
            _presenter.NativeModalViewControllerDisappearedOnItsOwn();
        }
    }
}