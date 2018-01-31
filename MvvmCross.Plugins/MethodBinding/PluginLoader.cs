// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.MethodBinding
{
    [Preserve(AllMembers = true)]
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
            host.Extensions.Add(new MvxMethodSourceBindingFactoryExtension());
        }
    }
}