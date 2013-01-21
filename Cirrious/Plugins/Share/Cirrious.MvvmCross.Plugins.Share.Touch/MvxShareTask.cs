// MvxShareTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform.Tasks;
using MonoTouch.Foundation;
using MonoTouch.Twitter;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Plugins.Share.Touch
{
    public class MvxShareTask
		: MvxTouchTask
		, IMvxShareTask
		, IMvxServiceConsumer
    {
        private readonly IMvxTouchViewPresenter _presenter;
        private TWTweetComposeViewController _tweet;

        public MvxShareTask()
        {
			_presenter = this.GetService<IMvxTouchViewPresenter>();
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