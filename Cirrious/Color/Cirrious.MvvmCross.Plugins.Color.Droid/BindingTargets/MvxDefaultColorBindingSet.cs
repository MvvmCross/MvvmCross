// MvxDefaultColorBindingSet.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxDefaultColorBindingSet
        : IMvxConsumer
    {
        public void RegisterBindings()
        {
            IMvxTargetBindingFactoryRegistry registry;
            if (!this.TryResolve(out registry))
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