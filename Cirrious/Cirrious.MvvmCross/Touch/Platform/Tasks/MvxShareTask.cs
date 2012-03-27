#region Copyright
// <copyright file="MvxShareTask.cs" company="Cirrious">
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
using MonoTouch.Twitter;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Platform.Tasks
{
    public class MvxShareTask : MvxTouchTask, IMvxShareTask
    {
        private readonly IMvxTouchViewPresenter _presenter;
        private TWTweetComposeViewController _tweet;

        public MvxShareTask (IMvxTouchViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public void ShareShort(string message)
        {
            if (!TWTweetComposeViewController.CanSendTweet)
                return;

            _tweet = new TWTweetComposeViewController();
            _tweet.SetInitialText(message);
            _tweet.SetCompletionHandler(TWTweetComposeHandler);
            _presenter.PresentModalViewController(_tweet, true);
        }

        public void ShareLink(string title, string message, string link)
        {
            if (!TWTweetComposeViewController.CanSendTweet)
                return;

            _tweet = new TWTweetComposeViewController();
            _tweet.SetInitialText(title + " " + message);
            _tweet.AddUrl(new NSUrl(link));
            _tweet.SetCompletionHandler(TWTweetComposeHandler);
            _presenter.PresentModalViewController(_tweet, true);
        }

        private void TWTweetComposeHandler(TWTweetComposeViewControllerResult result)
        {
            _presenter.NativeModalViewControllerDisappearedOnItsOwn();
            _tweet = null;
        }
    }
}