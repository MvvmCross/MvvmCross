// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Bindings.Target;
using MvvmCross.Forms.Views;

namespace MvvmCross.Forms.Core
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
