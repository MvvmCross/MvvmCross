using System;
using System.Threading;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.Conference.Core.ViewModels
{
    public class BaseViewModel
        : MvxViewModel
        , IMvxServiceConsumer<IMvxPhoneCallTask>
        , IMvxServiceConsumer<IMvxWebBrowserTask>
        , IMvxServiceConsumer<IMvxComposeEmailTask>
		, IMvxServiceConsumer<IMvxShareTask>
        , IMvxServiceConsumer<IErrorReporter>
    {
        public BaseViewModel()
        {
            ViewUnRegistered += (s, e) =>
                                    {
                                        if (!HasViews)
                                        {
                                            OnViewsDetached();
                                        }
                                    };
        }

        public virtual void OnViewsDetached()
        {
            // nothing to do in this base class
        }

        public IMvxLanguageBinder TextSource
        {
            get { return CreateLanguageBinder(Constants.GeneralNamespace, GetType().Name); }
        }

        public IMvxLanguageBinder SharedTextSource
        {
            get { return CreateLanguageBinder(Constants.GeneralNamespace, Constants.Shared); }
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
	
		public IMvxCommand ShareGeneralCommand
		{
			get { return new MvxRelayCommand(ShareGeneral); }
		}
		
		private void ShareGeneral()
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
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                Trace.Error("Exception masked in tweet " + exception.ToLongString());
            }
        }
    }
}