using System;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.Conference.Core.ViewModels
{
    public class BaseConferenceViewModel
        : BaseViewModel
        , IMvxServiceConsumer
    {
		private Guid _subscription;

        public BaseConferenceViewModel()
        {
			_subscription = Subscribe<LoadingChangedMessage>(message => RepositoryOnLoadingChanged());
        }

        public override void OnViewsDetached ()
		{
#warning DO NOT COPY THIS CODE - OnViewsDetached is not reliable on all platforms :(
			if (_subscription != Guid.Empty) {
				Unsubscribe<LoadingChangedMessage> (_subscription);
				_subscription = Guid.Empty;
			}

            base.OnViewsDetached();
        }

        public IConferenceService Service
        {
            get { return this.GetService<IConferenceService>(); }
        }

        public bool IsLoading
        {
            get { return Service.IsLoading; }
        }

        private void RepositoryOnLoadingChanged()
        {
            RaisePropertyChanged("IsSearching");
        }
    }
}