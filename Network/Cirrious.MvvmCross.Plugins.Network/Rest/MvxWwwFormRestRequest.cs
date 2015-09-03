// MvxWwwFormRestRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxWwwFormRestRequest<T>
        : MvxTextBasedRestRequest
    {
        public override bool NeedsRequestStream
        {
            get { return Parameters != null && Parameters.Count > 0; }
        }

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

        public MvxWwwFormRestRequest(string url, string verb = MvxVerbs.Post, string accept = MvxContentType.Json,
                                     string tag = null)
            : base(url, verb, accept, tag)
        {
            InitializeCommon();
        }

        public MvxWwwFormRestRequest(Uri url, string verb = MvxVerbs.Post, string accept = MvxContentType.Json,
                                     string tag = null)
            : base(url, verb, accept, tag)
        {
            InitializeCommon();
        }

        private void InitializeCommon()
        {
            Parameters = new Dictionary<string, object>();
            ContentType = MvxContentType.WwwForm;
        }
    }
}