// MvxDefaultColorBindingSet.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxDefaultColorBindingSet
    {
        public void RegisterBindings()
        {
            IMvxTargetBindingFactoryRegistry registry;
            if (!Mvx.TryResolve(out registry))
            {
                MvxTrace.Trace(MvxTraceLevel.Warning,
                               "No binding registry available - so color bindings will not be used");
                return;
            }

            registry.RegisterFactory(new MvxCustomBindingFactory<View>("BackgroundColor",
                                                                       view => new MvxViewBackgroundColorBinding(view)));
            registry.RegisterFactory(new MvxCustomBindingFactory<TextView>("TextColor",
                                                                           textView =>
                                                                           new MvxTextViewTextColorBinding(textView)));
        }
    }
}