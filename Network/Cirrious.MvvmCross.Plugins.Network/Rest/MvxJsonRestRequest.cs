// MvxJsonRestRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxJsonRestRequest<T>
        : MvxTextBasedRestRequest
        where T : class
    {
        public override bool NeedsRequestStream
        {
            get { return Body != null; }
        }

        public override void ProcessRequestStream(Stream stream)
        {
            var json = JsonConverterProvider().SerializeObject(Body);
            WriteTextToStream(stream, json);
        }

        public T Body { get; set; }
        public Func<IMvxJsonConverter> JsonConverterProvider { get; set; }

        public MvxJsonRestRequest(string url, string verb = MvxVerbs.Post, string accept = MvxContentType.Json,
                                  string tag = null)
            : base(url, verb, accept, tag)
        {
            InitializeCommon();
        }

        public MvxJsonRestRequest(Uri url, string verb = MvxVerbs.Post, string accept = MvxContentType.Json,
                                  string tag = null)
            : base(url, verb, accept, tag)
        {
            InitializeCommon();
        }

        private void InitializeCommon()
        {
            ContentType = MvxContentType.Json;
            JsonConverterProvider = () => Mvx.Resolve<IMvxJsonConverter>();
        }
    }
}