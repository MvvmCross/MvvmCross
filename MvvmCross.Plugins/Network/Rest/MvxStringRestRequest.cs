// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using MvvmCross.Platform;

namespace MvvmCross.Plugins.Network.Rest
{
    [Preserve(AllMembers = true)]
	public class MvxStringRestRequest
        : MvxTextBasedRestRequest
    {
        public MvxStringRestRequest(string url, string body = null, string verb = MvxVerbs.Post,
                                    string accept = MvxContentType.Json, string tag = null)
            : base(url, verb, accept, tag)
        {
            Body = body;
        }

        public MvxStringRestRequest(Uri uri, string body = null, string verb = MvxVerbs.Post,
                                    string accept = MvxContentType.Json, string tag = null)
            : base(uri, verb, accept, tag)
        {
            Body = body;
        }

        public override bool NeedsRequestStream => !string.IsNullOrEmpty(Body);

        public string Body { get; set; }

        public override void ProcessRequestStream(Stream stream)
        {
            WriteTextToStream(stream, Body);
        }
    }
}
