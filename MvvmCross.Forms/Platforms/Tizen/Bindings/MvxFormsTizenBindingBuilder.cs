// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Bindings;
using MvvmCross.Platforms.Tizen.Binding;

namespace MvvmCross.Forms.Platforms.Tizen.Bindings
{
    public class MvxFormsTizenBindingBuilder : MvxTizenBindingBuilder
    {
        public MvxFormsTizenBindingBuilder()
        {
        }

        public override void DoRegistration()
        {
            base.DoRegistration();
            InitializeBindingCreator();
        }

        protected override IMvxTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            return new MvxFormsTargetBindingFactoryRegistry();
        }

        private void InitializeBindingCreator()
        {
            var creator = CreateBindingCreator();
            Mvx.IoCProvider.RegisterSingleton(creator);
        }

        protected virtual IMvxBindingCreator CreateBindingCreator()
        {
            return new MvxFormsBindingCreator();
        }
    }
}
