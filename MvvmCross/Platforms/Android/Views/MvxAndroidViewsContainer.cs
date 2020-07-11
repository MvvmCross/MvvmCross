// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Android.Content;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxAndroidViewsContainer
        : MvxViewsContainer,
        IMvxAndroidViewsContainer
    {
        private const string ExtrasKey = "MvxLaunchData";
        private const string SubViewModelKey = "MvxSubViewModelKey";

        private readonly Context _applicationContext;

        public MvxAndroidViewsContainer(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #region Implementation of IMvxAndroidViewModelRequestTranslator

        public virtual ValueTask<IMvxViewModel?> Load(Intent intent, IMvxBundle? savedState)
        {
            return Load(intent, null, null);
        }

        public virtual async ValueTask<IMvxViewModel?> Load(Intent intent, IMvxBundle? savedState, Type? viewModelTypeHint)
        {
            if (intent == null)
            {
                MvxLog.Instance.Error("Null Intent seen when creating ViewModel");
                return null;
            }

            if (intent.Action == Intent.ActionMain)
            {
                MvxLog.Instance.Trace("Creating ViewModel for ActionMain");
                return await DirectLoad(savedState, viewModelTypeHint).ConfigureAwait(false);
            }

            if (intent.Extras == null)
            {
                MvxLog.Instance.Trace("Null Extras seen on Intent when creating ViewModel - have you tried to navigate to an MvvmCross View directly? Will try direct load");
                return await DirectLoad(savedState, viewModelTypeHint).ConfigureAwait(false);
            }

            if (TryGetEmbeddedViewModel(intent, out var mvxViewModel))
            {
                MvxLog.Instance.Trace("Embedded ViewModel used");
                return mvxViewModel;
            }

            MvxLog.Instance.Trace("Attempting to load new ViewModel from Intent with Extras");
            var toReturn = await CreateViewModelFromIntent(intent, savedState).ConfigureAwait(false);
            if (toReturn != null)
                return toReturn;

            MvxLog.Instance.Trace("ViewModel not loaded from Extras - will try DirectLoad");
            return await DirectLoad(savedState, viewModelTypeHint).ConfigureAwait(false);
        }

        protected virtual async ValueTask<IMvxViewModel?> DirectLoad(IMvxBundle? savedState, Type? viewModelTypeHint)
        {
            if (viewModelTypeHint == null)
            {
                MvxLog.Instance.Error("Unable to load viewmodel - no type hint provided");
                return null;
            }

            var loaderService = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            var viewModelRequest = MvxViewModelRequest.GetDefaultRequest(viewModelTypeHint);

            return await loaderService.LoadViewModel(viewModelRequest, savedState).ConfigureAwait(false);
        }

        protected virtual async ValueTask<IMvxViewModel?> CreateViewModelFromIntent(Intent intent, IMvxBundle? savedState)
        {
            var extraData = intent.Extras.GetString(ExtrasKey);
            if (extraData == null)
                return null;

            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            var viewModelRequest = converter.Serializer.DeserializeObject<MvxViewModelRequest>(extraData);

            return await ViewModelFromRequest(viewModelRequest, savedState).ConfigureAwait(false);
        }

        protected virtual ValueTask<IMvxViewModel> ViewModelFromRequest(MvxViewModelRequest viewModelRequest, IMvxBundle? savedState)
        {
            var loaderService = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            return loaderService.LoadViewModel(viewModelRequest, savedState);
        }

        protected virtual bool TryGetEmbeddedViewModel(Intent intent, out IMvxViewModel? mvxViewModel)
        {
            var embeddedViewModelKey = intent.Extras.GetInt(SubViewModelKey);
            if (embeddedViewModelKey != 0)
            {
                {
                    mvxViewModel = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>().Get(embeddedViewModelKey);
                    if (mvxViewModel != null)
                    {
                        RemoveSubViewModelWithKey(embeddedViewModelKey);
                        return true;
                    }
                }
            }
            mvxViewModel = null;
            return false;
        }

        public virtual Intent GetIntentFor(MvxViewModelRequest request)
        {
            if (request.ViewModelType == null) throw new NullReferenceException(nameof(request.ViewModelType));

            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            var requestText = converter.Serializer.SerializeObject(request);

            var intent = new Intent(_applicationContext, viewType);
            intent.PutExtra(ExtrasKey, requestText);

            AdjustIntentForPresentation(intent, request);

            return intent;
        }

        protected virtual void AdjustIntentForPresentation(Intent intent, MvxViewModelRequest request)
        {
            //todo we want to do things here... clear top, remove history item, etc
            //#warning ClearTop is not enough :/ Need to work on an Intent based scheme like http://stackoverflow.com/questions/3007998/on-logout-clear-activity-history-stack-preventing-back-button-from-opening-l
            //            if (request.ClearTop)
            //                intent.AddFlags(ActivityFlags.ClearTop);
        }

        public virtual (Intent intent, int key) GetIntentWithKeyFor(IMvxViewModel viewModel, MvxViewModelRequest request)
        {
            request = request ?? MvxViewModelRequest.GetDefaultRequest(viewModel.GetType());
            var intent = GetIntentFor(request);

            var childViewModelCache = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
            var key = childViewModelCache.Cache(viewModel);
            intent.PutExtra(SubViewModelKey, key);

            return (intent, key);
        }

        public void RemoveSubViewModelWithKey(int key)
        {
            Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>().Remove(key);
        }

        #endregion Implementation of IMvxAndroidViewModelRequestTranslator
    }
}
