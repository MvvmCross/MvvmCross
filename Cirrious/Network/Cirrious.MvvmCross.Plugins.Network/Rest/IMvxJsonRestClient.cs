using System;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public interface IMvxJsonRestClient
    {
        Func<IMvxJsonConverter> JsonConverterProvider { get; set; } 
        void MakeRequestFor<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction, Action<Exception> errorAction);
    }
}