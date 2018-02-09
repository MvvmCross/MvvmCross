// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Net;

namespace MvvmCross.Plugin.Network.Rest
{
    [Preserve(AllMembers = true)]
	public class MvxRestResponse
    {
        public string Tag { get; set; }
        public CookieCollection CookieCollection { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
