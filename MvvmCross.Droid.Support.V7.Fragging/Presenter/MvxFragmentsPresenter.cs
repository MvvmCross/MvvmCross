// MvxFragmentsPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.OS;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace MvvmCross.Droid.Support.V7.Fragging.Presenter
{
    public class MvxFragmentsPresenter
        : MvxAndroidViewPresenter
    {
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";

        private readonly FragmentHostRegistrationSettings _fragmentHostRegistrationSettings;
        private readonly Lazy<IMvxNavigationSerializer> _lazyNavigationSerializerFactory;

        protected IMvxNavigationSerializer Serializer => _lazyNavigationSerializerFactory.Value;

		public MvxFragmentsPresenter(IEnumerable<Assembly> AndroidViewAssemblies)
        {
            _lazyNavigationSerializerFactory = new Lazy<IMvxNavigationSerializer>(Mvx.Resolve<IMvxNavigationSerializer>);
			_fragmentHostRegistrationSettings = new FragmentHostRegistrationSettings(AndroidViewAssemblies);
        }

        public override sealed void Show(MvxViewModelRequest request)
        {
            if (_fragmentHostRegistrationSettings.IsTypeRegisteredAsFragment(request.ViewModelType))
                ShowFragment(request);
            else
                ShowActivity(request);
            
        }

        protected virtual void ShowActivity(MvxViewModelRequest request)
        {
            base.Show(request);
        }

        protected virtual void ShowFragment(MvxViewModelRequest request)
        {
            var bundle = new Bundle();
            var serializedRequest = Serializer.Serializer.SerializeObject(request);
            bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

            if (!_fragmentHostRegistrationSettings.IsActualHostValid(request.ViewModelType))
            {
                Type newFragmentHostViewModelType =
                    _fragmentHostRegistrationSettings.GetFragmentHostViewModelType(request.ViewModelType);

                var fragmentHostMvxViewModelRequest = MvxViewModelRequest.GetDefaultRequest(newFragmentHostViewModelType);
                ShowActivity(fragmentHostMvxViewModelRequest);
            }

            var mvxFragmentAttributeAssociated = _fragmentHostRegistrationSettings.GetMvxFragmentAttributeAssociated(request.ViewModelType);
            var fragmentType = _fragmentHostRegistrationSettings.GetFragmentTypeAssociatedWith(request.ViewModelType);
            GetActualFragmentHost().Show(request, bundle, fragmentType, mvxFragmentAttributeAssociated);
        }

        public override sealed void Close (IMvxViewModel viewModel)
        {
            if (_fragmentHostRegistrationSettings.IsTypeRegisteredAsFragment(viewModel.GetType()))
                CloseFragment(viewModel);
            else
                CloseActivity(viewModel);
        }

        protected virtual void CloseActivity(IMvxViewModel viewModel)
        {
            base.Close(viewModel);
        }

        protected virtual void CloseFragment(IMvxViewModel viewModel)
        {
            GetActualFragmentHost().Close(viewModel);
        }

        protected IMvxFragmentHost GetActualFragmentHost()
        {
            var currentActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            var fragmentHost = currentActivity as IMvxFragmentHost;

            if (fragmentHost == null)
                throw new InvalidOperationException($"You are trying to close ViewModel associated with Fragment when currently top Activity ({currentActivity.GetType()} does not implement IMvxFragmentHost interface!");

            return fragmentHost;
        }
         
    }
}