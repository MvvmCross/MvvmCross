// MvxStringRestRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;

namespace MvvmCross.Plugins.Network.Rest
{
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