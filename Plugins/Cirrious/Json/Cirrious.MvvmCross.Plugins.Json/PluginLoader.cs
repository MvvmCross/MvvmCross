// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class PluginLoader
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
                    MvxTrace.Error(
                                   "Error - multiple calls made to Json Plugin load while requesting different useJsonAsDefaultTextSerializer options");
                }
                return;
            }

            Mvx.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
            if (useJsonAsDefaultTextSerializer)
            {
                Mvx.RegisterType<IMvxTextSerializer, MvxJsonConverter>();
            }

            Mvx.RegisterType<IMvxJsonFlattener, MvxJsonFlattener>();
            _loaded = true;
        }
    }
}