// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Net;
using MvvmCross.Platform;

namespace MvvmCross.Plugin.Network.Rest
{
    [Preserve(AllMembers = true)]
	public class MvxRestRequestAsyncHandle : IMvxAbortable
    {
        private readonly HttpWebRequest _webRequest;

        public MvxRestRequestAsyncHandle(HttpWebRequest webRequest)
        {
            _webRequest = webRequest;
        }

        public void Abort()
        {
            _webRequest?.Abort();
        }
    }
}
