// IMvxRestClient.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public interface IMvxRestClient
    {
        void ClearSetting(string key);
        void SetSetting(string key, object value);

        MvxRestRequestAsyncHandle MakeRequest(MvxRestRequest restRequest, Action<MvxRestResponse> successAction,
                         Action<Exception> errorAction);

        MvxRestRequestAsyncHandle MakeRequest(MvxRestRequest restRequest, Action<MvxStreamRestResponse> successAction,
                         Action<Exception> errorAction);
    }
}