#region Copyright
// <copyright file="MvxAndroidViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using Android.Content;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Newtonsoft.Json;

#endregion

namespace Cirrious.MvvmCross.Android.Views
{
    public class MvxAndroidViewsContainer
        : MvxViewsContainer
        , IMvxAndroidViewModelLoader
        , IMvxAndroidViewModelRequestTranslator
        , IMvxServiceConsumer<IMvxAndroidCurrentTopActivity>
        , IMvxServiceConsumer<IMvxAndroidSubViewModelCache>
        , IMvxServiceConsumer<IMvxViewModelLoader>
    {
        private const string ExtrasKey = "MvxLaunchData";
        private const string SubViewModelKey = "MvxSubViewModelKey";

        private readonly Context _applicationContext;

        public MvxAndroidViewsContainer(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #region Implementation of IMvxAndroidViewModelRequestTranslator

        public virtual IMvxViewModel Load(Intent intent)
        {
            return Load(intent, null);
        }

        public virtual IMvxViewModel Load(Intent intent, Type viewModelTypeHint)
        {
            if (intent == null)
            {
                // TODO - some trace here would be nice...
                return null;
            }

            if (intent.Action == Intent.ActionMain)
            {
                // TODO - some trace here would be nice...
                return Activator.CreateInstance(viewModelTypeHint) as IMvxViewModel;
            }

            if (intent.Extras == null)
            {
                // TODO - some trace here would be nice...
                return null;
            }

            IMvxViewModel mvxViewModel;
            if (TryGetEmbeddedViewModel(intent, out mvxViewModel)) 
                return mvxViewModel;

            return CreateViewModelFromIntent(intent);
        }

        private IMvxViewModel CreateViewModelFromIntent(Intent intent)
        {
            var extraData = intent.Extras.GetString(ExtrasKey);
            if (extraData == null)
                return null;

            var viewModelRequest = JsonConvert.DeserializeObject<MvxShowViewModelRequest>(extraData);

            var loaderService = this.GetService<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest);
            return viewModel;
        }

        private bool TryGetEmbeddedViewModel(Intent intent, out IMvxViewModel mvxViewModel)
        {
            var embeddedViewModelKey = intent.Extras.GetInt(SubViewModelKey);
            if (embeddedViewModelKey != 0)
            {
                {
                    mvxViewModel = this.GetService<IMvxAndroidSubViewModelCache>().Get(embeddedViewModelKey);
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

            var requestText = JsonConvert.SerializeObject(request);

            var intent = new Intent(_applicationContext, viewType);
            intent.PutExtra(ExtrasKey, requestText);
#warning ClearTop is not enough :/ Need to work on an Intent based scheme like http://stackoverflow.com/questions/3007998/on-logout-clear-activity-history-stack-preventing-back-button-from-opening-l
            if (request.ClearTop)
                intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.NewTask);
            return intent;
        }

        public virtual Tuple<Intent,int> GetIntentWithKeyFor(IMvxViewModel viewModel)
        {
            var request = MvxShowViewModelRequest.GetDefaultRequest(viewModel.GetType());
            var intent = GetIntentFor(request);

            var key = this.GetService<IMvxAndroidSubViewModelCache>().Cache(viewModel);
            intent.PutExtra(SubViewModelKey, key);

            return new Tuple<Intent, int>(intent, key);
        }

        public void RemoveSubViewModelWithKey(int key)
        {
            this.GetService<IMvxAndroidSubViewModelCache>().Remove(key);
        }

        #endregion
    }
}