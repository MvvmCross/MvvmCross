// MvxShareTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using MvvmCross.Platform;
using MvvmCross.Platform.iOS.Platform;
using MvvmCross.Platform.iOS.Views;
using MvvmCross.iOS.Support.Views;
using Twitter;
using UIKit;

namespace MvvmCross.Plugins.Share.iOS
{
    [MvvmCross.Platform.Preserve(AllMembers = true)]
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
