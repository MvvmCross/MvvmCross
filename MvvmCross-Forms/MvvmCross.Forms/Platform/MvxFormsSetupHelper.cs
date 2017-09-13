using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Bindings.Target;
using MvvmCross.Forms.Views;

namespace MvvmCross.Forms.Platform
{
    public static class MvxFormsSetupHelper
    {
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxListViewItemClickPropertyTargetBinding),
                typeof(MvxListView), 
                MvxFormsPropertyBinding.MvxListView_ItemClick);
        }

        public static void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            
        }
    }
}
