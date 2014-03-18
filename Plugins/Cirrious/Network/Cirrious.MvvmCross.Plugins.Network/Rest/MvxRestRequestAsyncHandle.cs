using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public class MvxRestRequestAsyncHandle
    {
        public HttpWebRequest WebRequest;

        public MvxRestRequestAsyncHandle()
        {
        }

        public MvxRestRequestAsyncHandle(HttpWebRequest webRequest)
        {
            WebRequest = webRequest;
        }

        public void Abort()
        {
            if (WebRequest != null)
                WebRequest.Abort();
        }
    }
}
