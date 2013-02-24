// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class PluginLoader
        : IMvxProducer
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

            this.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
            if (useJsonAsDefaultTextSerializer)
            {
                this.RegisterType<IMvxTextSerializer, MvxJsonConverter>();
            }

            this.RegisterType<IMvxJsonFlattener, MvxJsonFlattener>();
            _loaded = true;
        }
    }
}