// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings.Source.Construction;

namespace MvvmCross.Plugin.MethodBinding
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        private bool _loaded;

        public void Load()
        {
            if (_loaded) return;

            Mvx.CallbackWhenRegistered<IMvxSourceBindingFactoryExtensionHost>(OnHostRegistered);
            _loaded = true;
        }

        private void OnHostRegistered(IMvxSourceBindingFactoryExtensionHost host)
        {
            host.Extensions.Add(new MvxMethodSourceBindingFactoryExtension());
        }
    }
}
