// MvxBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding
{
    public class MvxBindingBuilder : MvxCoreBindingBuilder
    {
        public override void DoRegistration()
        {
            base.DoRegistration();
            RegisterBindingFactories();
        }

        protected virtual void RegisterBindingFactories()
        {
            RegisterMvxBindingFactories();
        }

        protected virtual void RegisterMvxBindingFactories()
        {
            RegisterSourceStepFactory();
            RegisterPathSourceFactory();
            RegisterTargetFactory();
        }

        protected virtual void RegisterSourceStepFactory()
        {
            var sourceStepFactory = CreateSourceStepFactoryRegistry();
            FillSourceStepFactory(sourceStepFactory);
            Mvx.RegisterSingleton<IMvxSourceStepFactoryRegistry>(sourceStepFactory);
            Mvx.RegisterSingleton<IMvxSourceStepFactory>(sourceStepFactory);
        }

        protected virtual void FillSourceStepFactory(IMvxSourceStepFactoryRegistry registry)
        {
            registry.AddOrOverwrite(typeof(MvxCombinerSourceStepDescription), new MvxCombinerSourceStepFactory());
            registry.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            registry.AddOrOverwrite(typeof(MvxLiteralSourceStepDescription), new MvxLiteralSourceStepFactory());
        }

        protected virtual IMvxSourceStepFactoryRegistry CreateSourceStepFactoryRegistry()
        {
            return new MvxSourceStepFactory();
        }

        protected virtual void RegisterPathSourceFactory()
        {
            var sourceFactory = CreatePathSourceBindingFactory();
            Mvx.RegisterSingleton<IMvxPathSourceBindingFactory>(sourceFactory);
        }

        protected virtual IMvxPathSourceBindingFactory CreatePathSourceBindingFactory()
        {
            return new MvxPathSourceBindingFactory();
        }

        protected virtual void RegisterTargetFactory()
        {
            var targetRegistry = CreateTargetBindingRegistry();
            Mvx.RegisterSingleton<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            Mvx.RegisterSingleton<IMvxTargetBindingFactory>(targetRegistry);
            FillTargetFactories(targetRegistry);
        }

        protected virtual IMvxTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            return new MvxTargetBindingFactoryRegistry();
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // base class has nothing to register
        }
    }
}