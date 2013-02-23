using System;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Cirrious.Conference.Core.ViewModels
{
    public class BaseConferenceViewModel
        : BaseViewModel
        , IMvxServiceConsumer
    {
		private MvxSubscriptionToken _mvxSubscription;

        public BaseConferenceViewModel()
        {
			_mvxSubscription = Subscribe<LoadingChangedMessage>(message => RepositoryOnLoadingChanged());
        }

        public override void OnViewsDetached ()
		{
#warning DO NOT COPY THIS CODE - OnViewsDetached is not reliable on all platforms :(
			if (_mvxSubscription != null) {
				Unsubscribe<LoadingChangedMessage> (_mvxSubscription);
				_mvxSubscription = null;
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