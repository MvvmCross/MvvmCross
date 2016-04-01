// IMvxRestClient.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading.Tasks;

namespace MvvmCross.Plugins.Network.Rest
{
    public interface IMvxRestClient
    {
        void ClearSetting(string key);

        void SetSetting(string key, object value);
        
        Task<MvxRestResponse> MakeRequest(MvxRestRequest restRequest);
        Task<MvxStreamRestResponse> MakeStreamRequest(MvxRestRequest restRequest);
    }
}