using System;
using System.Collections.Generic;
using Android.OS;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.FullFragging.Presenter
{
    public class MvxFragmentsPresenter
        : MvxAndroidViewPresenter
        , IMvxFragmentsPresenter
    {
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";

        private readonly Dictionary<Type, IMvxFragmentHost> _dictionary = new Dictionary<Type, IMvxFragmentHost>();
        private IMvxNavigationSerializer _serializer;

        protected IMvxNavigationSerializer Serializer
        {
            get
            {
                if (_serializer != null)
                    return _serializer;

                _serializer = Mvx.Resolve<IMvxNavigationSerializer>();
                return _serializer;
            }
        }

        /// <summary>
        /// Register your ViewModel to be presented at a IMvxFragmentHost. Backingstore for this is a 
        /// Dictionary, hence you can only have a ViewModel registered at one IMvxFragmentHost at a time.
        /// Just call this method whenever you need to change the host.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the ViewModel to present</typeparam>
        /// <param name="host">Which IMvxFragmentHost (Activity) to present it at</param>
        public void RegisterViewModelAtHost<TViewModel>(IMvxFragmentHost host) 
            where TViewModel : IMvxViewModel
        {
            if (host == null)
            {
                Mvx.Warning("You passed a null IMvxFragmentHost, removing the registration instead");
                UnRegisterViewModelAtHost<TViewModel>();
            }
                
            _dictionary[typeof (TViewModel)] = host;
        }

        public void UnRegisterViewModelAtHost<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            _dictionary.Remove(typeof (TViewModel));
        }

        public override void Show(MvxViewModelRequest request)
        {
            var bundle = new Bundle();
            var serializedRequest = Serializer.Serializer.SerializeObject(request);
            bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

            IMvxFragmentHost host;
            if (_dictionary.TryGetValue(request.ViewModelType, out host))
            {
                if (host.Show(request, bundle))
                    return;
            }
            
            base.Show(request);
        }
    }
}