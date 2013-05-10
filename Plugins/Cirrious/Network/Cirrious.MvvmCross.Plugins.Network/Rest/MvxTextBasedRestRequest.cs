using System;
using System.IO;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public abstract class MvxTextBasedRestRequest
        : MvxRestRequest
    {
        protected MvxTextBasedRestRequest(string url, string verb = MvxVerbs.Get, string accept = MvxContentType.Json, string tag = null)
            : base(url, verb, accept, tag)
        {
        }

        protected MvxTextBasedRestRequest(Uri uri, string verb = MvxVerbs.Get, string accept = MvxContentType.Json, string tag = null)
            : base(uri, verb, accept, tag)
        {
        }

        protected static void WriteTextToStream(Stream stream, string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
    }
}