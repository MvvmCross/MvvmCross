using System;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.Conference.Core.ViewModels
{
    public class BaseConferenceViewModel
        : BaseViewModel
        , IMvxServiceConsumer<IConferenceService>
    {
        public BaseConferenceViewModel()
        {
            Service.LoadingChanged += RepositoryOnLoadingChanged;
        }

        public override void OnViewsDetached()
        {
            Service.LoadingChanged -= RepositoryOnLoadingChanged;
            
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

        private void RepositoryOnLoadingChanged(object sender, EventArgs eventArgs)
        {
            FirePropertyChanged("IsSearching");
        }
    }
}