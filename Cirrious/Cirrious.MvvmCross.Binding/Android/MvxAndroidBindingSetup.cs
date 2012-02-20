using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Binders;
using Cirrious.MvvmCross.Binding.Android.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Android.Target;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Android
{
    public class MvxAndroidBindingSetup 
        : MvxBindingSetup
        , IMvxServiceProducer<IMvxViewTypeResolver>
    {
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxEditTextTextTargetBinding), typeof(EditText), "Text"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxCompoundButtonCheckedTargetBinding), typeof (CompoundButton), "Checked"));
            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("AssetImagePath", (imageView) => new MvxImageViewDrawableTargetBinding(imageView)));
        }

        protected override void RegisterPlatformSpecificComponents()
        {
            base.RegisterPlatformSpecificComponents();

            this.RegisterServiceInstance<IMvxViewTypeResolver>(new MvxViewTypeResolver());
        }
    }
}