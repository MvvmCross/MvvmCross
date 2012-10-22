using Android.Content;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using CustomerManagement.Droid.Views;
using FooBar.Dialog.Droid;

namespace CustomerManagement.Droid
{
    public abstract class MvxBaseAndroidDialogBindingSetup
        : MvxBaseAndroidBindingSetup, IMvxServiceProducer
    {
        protected MvxBaseAndroidDialogBindingSetup(Context applicationContext) 
            : base(applicationContext)
        {
        }

        protected override void InitializeLastChance()
        {
            InitializeDialogBinding();
            base.InitializeLastChance();
        }

        protected virtual void InitializeDialogBinding()
        {
            DroidResources.Initialise(typeof(Resource.Layout));
        }

        protected override Cirrious.MvvmCross.Droid.Views.MvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            var container = base.CreateViewsContainer(applicationContext);
            var loader = CreateDefaultViewTextLoader();
            this.RegisterServiceInstance<IMvxDefaultViewTextLoader>(loader);
            var finder = MvxDefaultDialogViewFinder();
            container.AddSecondaryViewFinder(finder);
            return container;
        }

        protected virtual IMvxDefaultViewTextLoader CreateDefaultViewTextLoader()
        {
            return new MvxDefaultViewTextLoader();
        }

        protected virtual MvxDefaultDialogViewFinder MvxDefaultDialogViewFinder()
        {
            var finder = new MvxDefaultDialogViewFinder();
            return finder;
        }

        protected override void FillTargetFactories(Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterFactory(new MvxPropertyInfoTargetBindingFactory(typeof(ValueElement), "Value", (element, propertyInfo) => new MvxElementValueTargetBinding(element, propertyInfo)));
            base.FillTargetFactories(registry);
        }
    }
}