// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MvvmCross.Base;

namespace MvvmCross.Plugin.Network.Rest
{
    [Preserve(AllMembers = true)]
	public class MvxWwwFormRestRequest<T>
        : MvxTextBasedRestRequest
    {
        public override bool NeedsRequestStream => Parameters != null && Parameters.Count > 0;

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
