// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Plugin.Color.Platforms.Android.BindingTargets;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color.Platforms.Android
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public sealed class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();
            Mvx.IoCProvider.RegisterSingleton<IMvxNativeColor>(new MvxAndroidColor());
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(RegisterDefaultBindings);
        }

        private void RegisterDefaultBindings()
        {
            var helper = new MvxDefaultColorBindingSet();
            helper.RegisterBindings();
        }
    }
}
