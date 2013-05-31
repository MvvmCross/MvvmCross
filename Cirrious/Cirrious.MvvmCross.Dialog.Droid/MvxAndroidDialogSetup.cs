// MvxAndroidDialogSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Linq;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid.ResourceHelpers;
using Cirrious.MvvmCross.Dialog.Droid.Target;
using Cirrious.MvvmCross.Droid.Platform;
using CrossUI.Droid;
using CrossUI.Droid.Dialog.Elements;

namespace Cirrious.MvvmCross.Dialog.Droid
{
    public abstract class MvxAndroidDialogSetup
        : MvxAndroidSetup
    {
        protected MvxAndroidDialogSetup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override void InitialiseBindingBuilder()
        {
            base.InitialiseBindingBuilder();
            InitializeDialogBinding();
            InitializeUserInterfaceBuilder();
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
            DroidResources.Initialise(nestedResourceType);
        }

        protected override void FillTargetFactories(
            IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterFactory(new MvxPropertyInfoTargetBindingFactory(typeof (ValueElement), "Value",
                                                                             (element, propertyInfo) =>
                                                                             new MvxElementValueTargetBinding(element,
                                                                                                              propertyInfo)));
            base.FillTargetFactories(registry);
        }
    }
}