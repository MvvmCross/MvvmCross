using System.Net;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public class MvxRestResponse
    {
        public string Tag { get; set; }
        public CookieCollection CookieCollection { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}