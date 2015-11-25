// MvxRestRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxRestRequest
    {
        public MvxRestRequest(string url, string verb = MvxVerbs.Get, string accept = MvxContentType.Json,
                              string tag = null)
            : this(new Uri(url), verb, accept, tag)
        {
        }

        public MvxRestRequest(Uri uri, string verb = MvxVerbs.Get, string accept = MvxContentType.Json,
                              string tag = null)
        {
            Uri = uri;
            Tag = tag;
            Verb = verb;
            Accept = accept;
            Headers = new Dictionary<string, string>();
            Options = new Dictionary<string, object>();
        }

        public string Tag { get; set; }
        public Uri Uri { get; set; }
        public string Verb { get; set; }
        public string ContentType { get; set; }
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public Dictionary<string, object> Options { get; set; }
        public ICredentials Credentials { get; set; }

        public virtual bool NeedsRequestStream => false;

        public virtual void ProcessRequestStream(Stream stream)
        {
            // base class does nothing
        }
    }
}