using System;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.Network
{
    public class MvxJsonRestRequest<T>
        : MvxTextBasedRestRequest
        where T : class
    {
        public override bool NeedsRequestStream { get { return Body != null; } }

        public override void ProcessRequestStream(Stream stream)
        {
            var json = JsonConverterProvider().SerializeObject(Body);
            WriteTextToStream(stream, json);
        }

        public T Body { get; set; }
        public Func<IMvxJsonConverter> JsonConverterProvider { get; set; }

        public MvxJsonRestRequest(string url, string verb = MvxVerbs.Get, string tag = null)
            : base(url, verb, tag)
        {
            InitialiseCommon();
        }

        public MvxJsonRestRequest(Uri url, string verb = MvxVerbs.Get, string tag = null) 
            : base(url, verb, tag)
        {
            InitialiseCommon();
        }

        private void InitialiseCommon()
        {
            ContentType = MvxContentType.Json;
            JsonConverterProvider = () => Mvx.Resolve<IMvxJsonConverter>();
        }
    }
}