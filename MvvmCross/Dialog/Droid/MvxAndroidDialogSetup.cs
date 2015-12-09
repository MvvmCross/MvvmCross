// MvxAndroidDialogSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Droid
{
    using System.Linq;

    using Android.Content;

    using CrossUI.Droid;
    using CrossUI.Droid.Dialog.Elements;

    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Binding.Droid.ResourceHelpers;
    using MvvmCross.Dialog.Droid.Target;
    using MvvmCross.Platform;

    public abstract class MvxAndroidDialogSetup
        : MvxAndroidSetup
    {
        protected MvxAndroidDialogSetup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override void InitializeBindingBuilder()
        {
            base.InitializeBindingBuilder();
            this.InitializeDialogBinding();
            this.InitializeUserInterfaceBuilder();
        }

        private void InitializeUserInterfaceBuilder()
        {
            // TODO...
        }

        protected virtual void InitializeDialogBinding()
        {
            var finder = Mvx.Resolve<IMvxAppResourceTypeFinder>();
            var resourceType = finder.Find();
            var nestedResourceType = resourceType.GetNestedTypes().FirstOrDefault(x => x.Name == "Layout");
            DroidResources.Initialize(nestedResourceType);
        }

        protected override void FillTargetFactories(
            IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterFactory(new MvxPropertyInfoTargetBindingFactory(typeof(ValueElement), "Value",
                                                                             (element, propertyInfo) =>
                                                                             new MvxElementValueTargetBinding(element,
                                                                                                              propertyInfo)));
            base.FillTargetFactories(registry);
        }

        protected override void FillBindingNames(Binding.BindingContext.IMvxBindingNameRegistry registry)
        {
            registry.AddOrOverwrite(typeof(ValueElement), "Value");
            base.FillBindingNames(registry);
        }
    }
}