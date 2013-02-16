using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxDefaultColorBindingSet
        : IMvxServiceConsumer
    {
        public void RegisterBindings()
        {
            IMvxTargetBindingFactoryRegistry registry;
            if (!this.TryGetService<IMvxTargetBindingFactoryRegistry>(out registry))
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "No binding registry available - so color bindings will not be used");
                return;
            }

            registry.RegisterFactory(new MvxCustomBindingFactory<View>("BackgroundColor", view => new MvxViewBackgroundColorBinding(view)));
            registry.RegisterFactory(new MvxCustomBindingFactory<TextView>("TextColor", textView => new MvxTextViewTextColorBinding(textView)));
        }
    }
}