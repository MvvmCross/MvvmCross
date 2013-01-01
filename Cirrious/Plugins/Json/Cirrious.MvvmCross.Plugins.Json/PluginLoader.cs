// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class PluginLoader
        : IMvxServiceProducer
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        private bool _loaded;

        public void EnsureLoaded(bool useJsonAsDefaultTextSerializer = true)
        {
            if (_loaded)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning,
                               "Warning - you should really only initialize Json serialization once otherwise there is a risk of different clients requesting different useJsonAsDefaultTextSerializer options");
                return;
            }

            this.RegisterServiceType<IMvxJsonConverter, MvxJsonConverter>();
            if (useJsonAsDefaultTextSerializer)
            {
                this.RegisterServiceType<IMvxTextSerializer, MvxJsonConverter>();
            }

            this.RegisterServiceType<IMvxJsonFlattener, MvxJsonFlattener>();
            _loaded = true;
        }
    }
}