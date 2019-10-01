// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.IoC;

namespace MvvmCross.Plugin.MethodBinding
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxSourceBindingFactoryExtensionHost>(OnHostRegistered);
        }

        private void OnHostRegistered(IMvxSourceBindingFactoryExtensionHost host)
        {
            host.Extensions.Add(new MvxMethodSourceBindingFactoryExtension());
        }
    }
}
