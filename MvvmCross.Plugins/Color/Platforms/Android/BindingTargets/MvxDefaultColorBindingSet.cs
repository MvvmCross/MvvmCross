// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Logging;
using MvvmCross.Plugin.Color.Platforms.Android.Binding;

namespace MvvmCross.Plugin.Color.Platforms.Android.BindingTargets
{
    [Preserve(AllMembers = true)]
    public class MvxDefaultColorBindingSet
    {
        public void RegisterBindings()
        {
            IMvxTargetBindingFactoryRegistry registry;
            if (!Mvx.IoCProvider.TryResolve(out registry))
            {
                MvxPluginLog.Instance.Warn(
                               "No binding registry available - so color bindings will not be used");
                return;
            }

            registry.RegisterFactory(new MvxCustomBindingFactory<View>(
                MvxAndroidColorPropertyBinding.View_BackgroundColor,
                view => new MvxViewBackgroundColorBinding(view)));

            registry.RegisterFactory(new MvxCustomBindingFactory<TextView>(
                MvxAndroidColorPropertyBinding.TextView_TextColor,
                textView => new MvxTextViewTextColorBinding(textView)));
        }
    }
}
