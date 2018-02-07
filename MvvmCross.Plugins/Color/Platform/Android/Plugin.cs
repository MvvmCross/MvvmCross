// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Color.Droid.BindingTargets;

namespace MvvmCross.Plugin.Color.Platform.Android
{
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxPlugin          
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMvxNativeColor>(new MvxAndroidColor());
            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(RegisterDefaultBindings);
        }

        private void RegisterDefaultBindings()
        {
            var helper = new MvxDefaultColorBindingSet();
            helper.RegisterBindings();
        }
    }
}
