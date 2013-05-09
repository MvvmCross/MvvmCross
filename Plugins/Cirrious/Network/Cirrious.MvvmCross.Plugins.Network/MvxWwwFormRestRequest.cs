using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.Network
{
    public class MvxWwwFormRestRequest<T>
        : MvxTextBasedRestRequest
    {
        public override bool NeedsRequestStream { get { return Parameters != null && Parameters.Count > 0; } }
        public Dictionary<string, object> Parameters { get; set; }

        public override void ProcessRequestStream(Stream stream)
        {
            var text = new StringBuilder();
            foreach (var kvp in Parameters)
            {
                if (text.Length > 0)
                    text.Append("&");

                text.Append(kvp.Key);
                text.Append("=");
                text.Append(Uri.EscapeDataString(kvp.Value.ToString()));
            }
            WriteTextToStream(stream, text.ToString());
        }

        public MvxWwwFormRestRequest(string url, string verb = MvxVerbs.Get, string tag = null)
            : base(url, verb, tag)
        {
            InitialiseCommon();
        }

        public MvxWwwFormRestRequest(Uri url, string verb = MvxVerbs.Get, string tag = null) 
            : base(url, verb, tag)
        {
            InitialiseCommon();
        }

        private void InitialiseCommon()
        {
            Parameters = new Dictionary<string, object>();
            ContentType = MvxContentType.WwwForm;
        }
    }
}