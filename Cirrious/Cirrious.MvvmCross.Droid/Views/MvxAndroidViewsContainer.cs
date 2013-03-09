﻿// MvxAndroidViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidViewsContainer
        : MvxViewsContainer
          , IMvxAndroidViewModelLoader
          , IMvxAndroidViewModelRequestTranslator
    {
        private const string ExtrasKey = "MvxLaunchData";
        private const string SubViewModelKey = "MvxSubViewModelKey";

        private readonly Context _applicationContext;

        public MvxAndroidViewsContainer(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #region Implementation of IMvxAndroidViewModelRequestTranslator

        public virtual IMvxViewModel Load(Intent intent, IMvxBundle savedState)
        {
            return Load(intent, null);
        }

        public virtual IMvxViewModel Load(Intent intent, IMvxBundle savedState, Type viewModelTypeHint)
        {
            if (intent == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, "Null Intent seen when creating ViewModel");
                return null;
            }

            if (intent.Action == Intent.ActionMain)
            {
                MvxTrace.Trace("Creating ViewModel for ActionMain");
                return Activator.CreateInstance(viewModelTypeHint) as IMvxViewModel;
            }

            if (intent.Extras == null)
            {
                MvxTrace.Trace(MvxTraceLevel.Error,
                               "Null Extras seen on Intent when creating ViewModel - this should not happen - have you tried to navigate to an MvvmCross View directly?");
                return null;
            }

            IMvxViewModel mvxViewModel;
            if (TryGetEmbeddedViewModel(intent, out mvxViewModel))
            {
                MvxTrace.Trace("Embedded ViewModel used");
                return mvxViewModel;
            }

            MvxTrace.Trace("Loading new ViewModel from Intent with Extras");
            return CreateViewModelFromIntent(intent, savedState);
        }

        private IMvxViewModel CreateViewModelFromIntent(Intent intent, IMvxBundle savedState)
        {
            var extraData = intent.Extras.GetString(ExtrasKey);
            if (extraData == null)
                return null;

            var converter = Mvx.Resolve<IMvxNavigationRequestSerializer>();
            var viewModelRequest = converter.Serializer.DeserializeObject<MvxShowViewModelRequest>(extraData);

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest, savedState);
            return viewModel;
        }

        private bool TryGetEmbeddedViewModel(Intent intent, out IMvxViewModel mvxViewModel)
        {
            var embeddedViewModelKey = intent.Extras.GetInt(SubViewModelKey);
            if (embeddedViewModelKey != 0)
            {
                {
                    mvxViewModel = Mvx.Resolve<IMvxAndroidSubViewModelCache>().Get(embeddedViewModelKey);
                    return true;
                }
            }
            mvxViewModel = null;
            return false;
        }

        public virtual Intent GetIntentFor(MvxShowViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

            var converter = Mvx.Resolve<IMvxNavigationRequestSerializer>();
            var requestText = converter.Serializer.SerializeObject(request);

            var intent = new Intent(_applicationContext, viewType);
            intent.PutExtra(ExtrasKey, requestText);
#warning ClearTop is not enough :/ Need to work on an Intent based scheme like http://stackoverflow.com/questions/3007998/on-logout-clear-activity-history-stack-preventing-back-button-from-opening-l
            if (request.ClearTop)
                intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.NewTask);
            return intent;
        }

        public virtual Tuple<Intent, int> GetIntentWithKeyFor(IMvxViewModel viewModel)
        {
            var request = MvxShowViewModelRequest.GetDefaultRequest(viewModel.GetType());
            var intent = GetIntentFor(request);

            var key = Mvx.Resolve<IMvxAndroidSubViewModelCache>().Cache(viewModel);
            intent.PutExtra(SubViewModelKey, key);

            return new Tuple<Intent, int>(intent, key);
        }

        public void RemoveSubViewModelWithKey(int key)
        {
            Mvx.Resolve<IMvxAndroidSubViewModelCache>().Remove(key);
        }

        #endregion
    }
}