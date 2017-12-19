// MvxWindowsBindingBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.iOS;
using MvvmCross.Platform;
using MvvmCross.Forms.Bindings;

namespace MvvmCross.Forms.iOS.Bindings
{
    public class MvxFormsIosBindingBuilder : MvxIosBindingBuilder
    {
        public MvxFormsIosBindingBuilder()
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

        protected virtual IMvxBindingCreator CreateBindingCreator()
        {
            return new MvxFormsBindingCreator();
        }
    }
}
