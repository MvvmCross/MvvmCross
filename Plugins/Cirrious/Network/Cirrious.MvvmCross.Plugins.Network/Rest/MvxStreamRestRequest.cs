// MvxStreamRestRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
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

        public override bool NeedsRequestStream
        {
            get { return BodyHandler != null; }
        }

        public Action<Stream> BodyHandler { get; set; }

        public override void ProcessRequestStream(Stream stream)
        {
            BodyHandler(stream);
        }
    }
}