using System;
using System.IO;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public class MvxStringRestRequest
        : MvxTextBasedRestRequest
    {
        public MvxStringRestRequest(string url, string body = null, string verb = MvxVerbs.Post, string accept = MvxContentType.Json, string tag = null)
            : base(url, verb, accept, tag)
        {
            Body = body;
        }

        public MvxStringRestRequest(Uri uri, string body = null, string verb = MvxVerbs.Post, string accept = MvxContentType.Json, string tag = null)
            : base(uri, verb, accept, tag)
        {
            Body = body;
        }

        public override bool NeedsRequestStream { get { return !string.IsNullOrEmpty(Body); } }
        public string Body { get; set; }

        public override void ProcessRequestStream(Stream stream)
        {
            WriteTextToStream(stream, Body);
        }
    }
}