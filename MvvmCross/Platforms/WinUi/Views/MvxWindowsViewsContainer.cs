// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.WinUi.Views
{
    public class MvxWindowsViewsContainer
        : MvxViewsContainer
        , IMvxStoreViewsContainer
    {
        private const string ExtrasKey = "MvxLaunchData";

        public IMvxViewModel Load(string requestText, IMvxBundle savedState)
        {
            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            var dictionary = converter.Serializer.DeserializeObject<Dictionary<string, string>>(requestText);

            dictionary.TryGetValue(ExtrasKey, out string serializedRequest);
            var request = converter.Serializer.DeserializeObject<MvxViewModelRequest>(serializedRequest);

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

        public string GetRequestTextFor(IMvxViewModel existingViewModelToUse)
        {
            var returnData = new Dictionary<string, string>();
            var converter = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
            var request = MvxViewModelRequest.GetDefaultRequest(existingViewModelToUse.GetType());

            returnData.Add(ExtrasKey, converter.Serializer.SerializeObject(request));

            var requestText = converter.Serializer.SerializeObject(returnData);

            return requestText;
        }
        #endregion Implementation of IMvxWindowsViewModelRequestTranslator
    }
}
