// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Uwp;
using MvvmCross.Platform;
using MvvmCross.Forms.Bindings;

namespace MvvmCross.Forms.Platform.Uap.Bindings
{
    public class MvxFormsWindowsBindingBuilder : MvxWindowsBindingBuilder
    {
        public MvxFormsWindowsBindingBuilder()
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
            Mvx.RegisterSingleton(creator);
        }

        protected new virtual Forms.Bindings.IMvxBindingCreator CreateBindingCreator()
        {
            return new MvxFormsBindingCreator();
        }
    }
}
