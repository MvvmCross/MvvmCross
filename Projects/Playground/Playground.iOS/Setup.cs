using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Ios.Core;
using Playground.Core;
using Playground.iOS.Bindings;
using Playground.iOS.Controls;

namespace Playground.iOS
{
    public class Setup : MvxIosSetup<App>
    {

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<BinaryEdit>(
                "MyCount",
                (arg) => new BinaryEditTargetBinding(arg));

            base.FillTargetFactories(registry);
        }


    }
}
