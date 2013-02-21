using System;
using System.Threading;
using System.Windows.Input;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Localization.Interfaces;
using Cirrious.MvvmCross.Plugins.Email;
using Cirrious.MvvmCross.Plugins.PhoneCall;
using Cirrious.MvvmCross.Plugins.Share;
using Cirrious.MvvmCross.Plugins.WebBrowser;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cirrious.Conference.Core.ViewModels
{
    public class BaseViewModel
        : MvxViewModel
        , IMvxServiceConsumer
	{
        public BaseViewModel()
        {
#warning DO NOT COPY THIS CODE - ViewUnRegistered is not reliable on all platforms :(
            ViewUnRegistered += (s, e) =>
                                    {
                                        if (!HasViews)
                                        {
                                            OnViewsDetached();
                                        }
                                    };
        }

		private IMessenger Messenger {
			get {
				return this.GetService<IMessenger>();
			}
		}

		protected SubscriptionToken Subscribe<TMessage> (Action<TMessage> action)
			where TMessage : BaseMessage
		{
			return Messenger.Subscribe<TMessage>(action, false /* weak reference */);
		}

		protected void Unsubscribe<TMessage> (SubscriptionToken id)
			where TMessage : BaseMessage
		{
			Messenger.Unsubscribe<TMessage>(id);
		}

        public virtual void OnViewsDetached()
        {
            // nothing to do in this base class
        }

        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder(Constants.GeneralNamespace, GetType().Name); }
        }

        public IMvxLanguageBinder SharedTextSource
        {
            get { return new MvxLanguageBinder(Constants.GeneralNamespace, Constants.Shared); }
        }

        protected void ReportError(string text)
        {
            this.GetService<IErrorReporter>().ReportError(text);
        }

        protected void MakePhoneCall(string name, string number)
        {
            var task = this.GetService<IMvxPhoneCallTask>();
            task.MakePhoneCall(name, number);
        }

        protected void ShowWebPage(string webPage)
        {
            var task = this.GetService<IMvxWebBrowserTask>();
            task.ShowWebPage(webPage);
        }

        protected void ComposeEmail(string to, string subject, string body)
        {
            var task = this.GetService<IMvxComposeEmailTask>();
            task.ComposeEmail(to, null, subject, body, false);
        }
	
		public ICommand ShareGeneralCommand
		{
			get { return new MvxRelayCommand(DoShareGeneral); }
		}
		
		public void DoShareGeneral()
		{
            var toShare = string.Format("#SQLBitsX");
		    ExceptionSafeShare(toShare);
		}

        protected void ExceptionSafeShare(string toShare)
        {
            try
            {
                var service = this.GetService<IMvxShareTask>();
                service.ShareShort(toShare);
            }
            catch (Exception exception)
            {
                Trace.Error("Exception masked in tweet " + exception.ToLongString());
            }
        }
    }
}