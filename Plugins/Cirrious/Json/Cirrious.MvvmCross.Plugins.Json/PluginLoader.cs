// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class PluginLoader
        : IMvxServiceProducer
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        private bool _loaded;
        private bool _loadedOption;

        public void EnsureLoaded(bool useJsonAsDefaultTextSerializer = true)
        {
            if (_loaded)
            {
                if (useJsonAsDefaultTextSerializer != _loadedOption)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error,
                                   "Error - multiple calls made to Json Plugin load while requesting different useJsonAsDefaultTextSerializer options");
                }
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