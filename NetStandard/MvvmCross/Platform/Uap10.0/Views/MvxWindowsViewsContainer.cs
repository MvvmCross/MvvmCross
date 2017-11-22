// MvxStoreViewsContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;

namespace MvvmCross.Uwp.Views
{
    public class MvxWindowsViewsContainer
        : MvxViewsContainer
        , IMvxStoreViewsContainer
    {
        private const string ExtrasKey = "MvxLaunchData";
        private const string SubViewModelKey = "MvxSubViewModelKey";

        public IMvxViewModel Load(string requestText, IMvxBundle savedState)
        {
            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var dictionary = converter.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

            dictionary.TryGetValue(ExtrasKey, out string serializedRequest);
            var request = converter.Serializer.DeserializeObject<MvxViewModelRequest>(serializedRequest);

            if (dictionary.TryGetValue(SubViewModelKey, out string viewModelKey))
            {
                var key = int.Parse(viewModelKey);
                var viewModel = Mvx.Resolve<IMvxChildViewModelCache>().Get(key);
                return viewModel;
            }

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            return loaderService.LoadViewModel(request, savedState);
        }

        #region Implementation of IMvxWindowsViewModelRequestTranslator
        public string GetRequestTextFor(MvxViewModelRequest request)
        {
            var returnData = new Dictionary<string, string>();
            var converter = Mvx.Resolve<IMvxNavigationSerializer>();

            returnData.Add(ExtrasKey, converter.Serializer.SerializeObject(request));

            var requestText = converter.Serializer.SerializeObject(returnData);
            return requestText;
        }

        public string GetRequestTextWithKeyFor(IMvxViewModel viewModel)
        {
            var returnData = new Dictionary<string, string>();
            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var request = MvxViewModelRequest.GetDefaultRequest(viewModel.GetType());

            var key = Mvx.Resolve<IMvxChildViewModelCache>().Cache(viewModel);
            returnData.Add(ExtrasKey, converter.Serializer.SerializeObject(request));
            returnData.Add(SubViewModelKey, key.ToString());

            var requestText = converter.Serializer.SerializeObject(returnData);

            return requestText;
        }

        public void RemoveSubViewModelWithKey(int key)
        {
            Mvx.Resolve<IMvxChildViewModelCache>().Remove(key);
        }

        public int RequestTextGetKey(string requestText)
        {
            var returnValue = 0;
            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
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