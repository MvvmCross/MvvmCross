// MvxRestResponse.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Net;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxRestResponse
    {
        public string Tag { get; set; }
        public CookieCollection CookieCollection { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}