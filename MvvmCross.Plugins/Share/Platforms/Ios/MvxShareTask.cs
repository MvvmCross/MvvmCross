// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Foundation;
using MvvmCross.Platforms.Ios;
using MvvmCross.Platforms.Ios.Views;
using Twitter;
using UIKit;

namespace MvvmCross.Plugin.Share.Platforms.Ios
{
    [MvvmCross.Preserve(AllMembers = true)]
    public class MvxShareTask
        : MvxIosTask, IMvxShareTask
    {
        private TWTweetComposeViewController _tweet;

        public MvxShareTask()
        {
        }

        public void ShareShort(string message)
        {
            if (!TWTweetComposeViewController.CanSendTweet)
                return;

            _tweet = new TWTweetComposeViewController();
            _tweet.SetInitialText(message);
            _tweet.CompletionHandler = TWTweetComposeHandler;

            UIApplication.SharedApplication.KeyWindow.GetTopModalHostViewController().PresentViewController(_tweet, true, null);
        }

        public void ShareLink(string title, string message, string link)
        {
            if (!TWTweetComposeViewController.CanSendTweet)
                return;

            _tweet = new TWTweetComposeViewController();
            _tweet.SetInitialText(title + " " + message);
            _tweet.AddUrl(new NSUrl(link));
            _tweet.CompletionHandler = TWTweetComposeHandler;

            UIApplication.SharedApplication.KeyWindow.GetTopModalHostViewController().PresentViewController(_tweet, true, null);
        }

        private void TWTweetComposeHandler(TWTweetComposeViewControllerResult result)
        {
            _tweet.DismissViewController(true, () => { });
            _tweet = null;
        }
    }
}
