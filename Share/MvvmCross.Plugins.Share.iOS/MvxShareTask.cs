// MvxShareTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.iOS.Platform;
using MvvmCross.Platform.iOS.Views;
using Foundation;
using Twitter;

namespace MvvmCross.Plugins.Share.iOS
{
    public class MvxShareTask
        : MvxIosTask
          , IMvxShareTask

    {
        private readonly IMvxIosModalHost _modalHost;
        private TWTweetComposeViewController _tweet;

        public MvxShareTask()
        {
            _modalHost = Mvx.Resolve<IMvxIosModalHost>();
        }

        public void ShareShort(string message)
        {
            if (!TWTweetComposeViewController.CanSendTweet)
                return;

            _tweet = new TWTweetComposeViewController();
            _tweet.SetInitialText(message);
            _tweet.SetCompletionHandler(TWTweetComposeHandler);
            _modalHost.PresentModalViewController(_tweet, true);
        }

        public void ShareLink(string title, string message, string link)
        {
            if (!TWTweetComposeViewController.CanSendTweet)
                return;

            _tweet = new TWTweetComposeViewController();
            _tweet.SetInitialText(title + " " + message);
            _tweet.AddUrl(new NSUrl(link));
            _tweet.SetCompletionHandler(TWTweetComposeHandler);
            _modalHost.PresentModalViewController(_tweet, true);
        }

        private void TWTweetComposeHandler(TWTweetComposeViewControllerResult result)
        {
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
            _tweet = null;
        }
    }
}