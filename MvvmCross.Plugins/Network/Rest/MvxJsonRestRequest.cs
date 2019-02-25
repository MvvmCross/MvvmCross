// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using MvvmCross.Base;

namespace MvvmCross.Plugin.Network.Rest
{
    [Preserve(AllMembers = true)]
	public class MvxJsonRestRequest<T>
        : MvxTextBasedRestRequest
        where T : class
    {
        public override bool NeedsRequestStream => Body != null;

        public override void ProcessRequestStream(Stream stream)
        {
            var json = JsonConverterProvider?.Invoke().SerializeObject(Body);
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
            JsonConverterProvider = () => Mvx.IoCProvider.Resolve<IMvxJsonConverter>();
        }
    }
}
