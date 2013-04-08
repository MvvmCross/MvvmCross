using System;
using System.Threading;
using System.Windows.Input;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Localization;
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
        
	{
		private IMvxMessenger MvxMessenger {
			get {
				return Mvx.Resolve<IMvxMessenger>();
			}
		}

		protected MvxSubscriptionToken Subscribe<TMessage> (Action<TMessage> action)
			where TMessage : MvxMessage
		{
		    return MvxMessenger.Subscribe<TMessage>(action, MvxReference.Weak);
		}

		protected void Unsubscribe<TMessage> (MvxSubscriptionToken id)
			where TMessage : MvxMessage
		{
			MvxMessenger.Unsubscribe<TMessage>(id);
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
            Mvx.Resolve<IErrorReporter>().ReportError(text);
        }

        protected void MakePhoneCall(string name, string number)
        {
            var task = Mvx.Resolve<IMvxPhoneCallTask>();
            task.MakePhoneCall(name, number);
        }

        protected void ShowWebPage(string webPage)
        {
            var task = Mvx.Resolve<IMvxWebBrowserTask>();
            task.ShowWebPage(webPage);
        }

        protected void ComposeEmail(string to, string subject, string body)
        {
            var task = Mvx.Resolve<IMvxComposeEmailTask>();
            task.ComposeEmail(to, null, subject, body, false);
        }
	
		public ICommand ShareGeneralCommand
		{
			get { return new MvxCommand(DoShareGeneral); }
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
                var service = Mvx.Resolve<IMvxShareTask>();
                service.ShareShort(toShare);
            }
            catch (Exception exception)
            {
                Trace.Error("Exception masked in tweet " + exception.ToLongString());
            }
        }
    }
}