// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Uap.Views
{
    public class MvxWindowsViewsContainer
        : MvxViewsContainer
        , IMvxStoreViewsContainer
    {
        private const string ExtrasKey = "MvxLaunchData";
        private const string SubViewModelKey = "MvxSubViewModelKey";

        public IMvxViewModel Load(string requestText, IMvxBundle savedState)
        {
            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            var dictionary = converter.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

            dictionary.TryGetValue(ExtrasKey, out string serializedRequest);
            var request = converter.Serializer.DeserializeObject<MvxViewModelRequest>(serializedRequest);

            if (dictionary.TryGetValue(SubViewModelKey, out string viewModelKey))
            {
                var key = int.Parse(viewModelKey);
                var viewModel = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>().Get(key);
                if (savedState != null)
                    viewModel.ReloadState(savedState);
                return viewModel;
            }

            var loaderService = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            return loaderService.LoadViewModel(request, savedState);
        }

        #region Implementation of IMvxWindowsViewModelRequestTranslator
        public string GetRequestTextFor(MvxViewModelRequest request)
        {
            var returnData = new Dictionary<string, string>();
            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();

            returnData.Add(ExtrasKey, converter.Serializer.SerializeObject(request));

            var requestText = converter.Serializer.SerializeObject(returnData);
            return requestText;
        }

        public string GetRequestTextWithKeyFor(IMvxViewModel viewModel)
        {
            var returnData = new Dictionary<string, string>();
            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            var request = MvxViewModelRequest.GetDefaultRequest(viewModel.GetType());

            var key = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>().Cache(viewModel);
            returnData.Add(ExtrasKey, converter.Serializer.SerializeObject(request));
            returnData.Add(SubViewModelKey, key.ToString());

            var requestText = converter.Serializer.SerializeObject(returnData);

            return requestText;
        }

        public void RemoveSubViewModelWithKey(int key)
        {
            Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>().Remove(key);
        }

        public int RequestTextGetKey(string requestText)
        {
            var returnValue = 0;
            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            var dictionary = converter.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

            dictionary.TryGetValue(ExtrasKey, out string serializedRequest);
            var request = converter.Serializer.DeserializeObject<MvxViewModelRequest>(serializedRequest);

            if (dictionary.TryGetValue(SubViewModelKey, out string viewModelKey))
            {
                returnValue = int.Parse(viewModelKey);
            }
            return returnValue;
        }
        #endregion Implementation of IMvxWindowsViewModelRequestTranslator
    }
}
