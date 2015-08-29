// IMvxJsonRestClient.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public interface IMvxJsonRestClient
    {
        Func<IMvxJsonConverter> JsonConverterProvider { get; set; }

        IMvxAbortable MakeRequestFor<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction,
                               Action<Exception> errorAction);
    }
}