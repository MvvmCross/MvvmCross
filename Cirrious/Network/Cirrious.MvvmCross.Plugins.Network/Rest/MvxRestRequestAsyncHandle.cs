// MvxRestRequestAsyncHandle.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Net;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    // Credit - this class heavily influenced by the wonderful https://github.com/restsharp/RestSharp
    //        - credit here under Apache 2.0 license
    public class MvxRestRequestAsyncHandle
        : IMvxAbortable
    {
        private readonly HttpWebRequest _webRequest;

        public MvxRestRequestAsyncHandle(HttpWebRequest webRequest)
        {
            _webRequest = webRequest;
        }

        public void Abort()
        {
            if (_webRequest != null)
                _webRequest.Abort();
        }
    }
}
