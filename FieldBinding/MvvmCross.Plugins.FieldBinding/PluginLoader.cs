// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;

namespace MvvmCross.Plugins.FieldBinding
{
    public class PluginLoader
        : IMvxPluginLoader
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        private bool _loaded;

        public void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }

            Mvx.CallbackWhenRegistered<IMvxSourceBindingFactoryExtensionHost>(OnHostRegistered);
            _loaded = true;
        }

        private void OnHostRegistered(IMvxSourceBindingFactoryExtensionHost host)
        {
            host.Extensions.Add(new MvxFieldSourceBindingFactoryExtension());
        }
    }
}