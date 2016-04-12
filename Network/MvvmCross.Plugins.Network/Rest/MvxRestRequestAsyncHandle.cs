using System.Net;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxRestRequestAsyncHandle: IMvxAbortable
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