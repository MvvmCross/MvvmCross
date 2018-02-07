// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using MvvmCross.Platform;

namespace MvvmCross.Plugin.Network.Rest
{
    [Preserve(AllMembers = true)]
	public class MvxStreamRestRequest
        : MvxRestRequest
    {
        public MvxStreamRestRequest(string url, Action<Stream> streamAction = null, string verb = MvxVerbs.Post,
                                    string accept = MvxContentType.Json, string tag = null)
            : base(url, verb, accept, tag)
        {
            BodyHandler = streamAction;
        }

        public MvxStreamRestRequest(Uri uri, Action<Stream> streamAction = null, string verb = MvxVerbs.Post,
                                    string accept = MvxContentType.Json, string tag = null)
            : base(uri, verb, accept, tag)
        {
            BodyHandler = streamAction;
        }

        public override bool NeedsRequestStream => BodyHandler != null;

        public Action<Stream> BodyHandler { get; set; }

        public override void ProcessRequestStream(Stream stream)
        {
            BodyHandler?.Invoke(stream);
        }
    }
}
