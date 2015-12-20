// MvxBindingBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding
{
    using MvvmCross.Binding.Bindings.Source.Construction;
    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Platform;

    public class MvxBindingBuilder : MvxCoreBindingBuilder
    {
        public override void DoRegistration()
        {
            base.DoRegistration();
            this.RegisterBindingFactories();
        }

        protected virtual void RegisterBindingFactories()
        {
            this.RegisterMvxBindingFactories();
        }

        protected virtual void RegisterMvxBindingFactories()
        {
            this.RegisterSourceStepFactory();
            this.RegisterSourceFactory();
            this.RegisterTargetFactory();
        }

        protected virtual void RegisterSourceStepFactory()
        {
            var sourceStepFactory = this.CreateSourceStepFactoryRegistry();
            this.FillSourceStepFactory(sourceStepFactory);
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

        protected virtual void RegisterSourceFactory()
        {
            var sourceFactory = this.CreateSourceBindingFactory();
            Mvx.RegisterSingleton<IMvxSourceBindingFactory>(sourceFactory);
            var extensionHost = sourceFactory as IMvxSourceBindingFactoryExtensionHost;
            if (extensionHost != null)
            {
                this.RegisterSourceBindingFactoryExtensions(extensionHost);
                Mvx.RegisterSingleton<IMvxSourceBindingFactoryExtensionHost>(extensionHost);
            }
            else
                Mvx.Trace("source binding factory extension host not provided - so no source extensions will be used");
        }

        protected virtual void RegisterSourceBindingFactoryExtensions(IMvxSourceBindingFactoryExtensionHost extensionHost)
        {
            extensionHost.Extensions.Add(new MvxPropertySourceBindingFactoryExtension());
        }

        protected virtual IMvxSourceBindingFactory CreateSourceBindingFactory()
        {
            return new MvxSourceBindingFactory();
        }

        protected virtual void RegisterTargetFactory()
        {
            var targetRegistry = this.CreateTargetBindingRegistry();
            Mvx.RegisterSingleton<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            Mvx.RegisterSingleton<IMvxTargetBindingFactory>(targetRegistry);
            this.FillTargetFactories(targetRegistry);
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