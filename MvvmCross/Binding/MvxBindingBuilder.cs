// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Logging;

namespace MvvmCross.Binding
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
            RegisterSourceFactory();
            RegisterTargetFactory();
        }

        protected virtual void RegisterSourceStepFactory()
        {
            var sourceStepFactory = CreateSourceStepFactoryRegistry();
            FillSourceStepFactory(sourceStepFactory);
            Mvx.IoCProvider.RegisterSingleton<IMvxSourceStepFactoryRegistry>(sourceStepFactory);
            Mvx.IoCProvider.RegisterSingleton<IMvxSourceStepFactory>(sourceStepFactory);
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
            var sourceFactory = CreateSourceBindingFactory();
            Mvx.IoCProvider.RegisterSingleton<IMvxSourceBindingFactory>(sourceFactory);
            var extensionHost = sourceFactory as IMvxSourceBindingFactoryExtensionHost;
            if (extensionHost != null)
            {
                RegisterSourceBindingFactoryExtensions(extensionHost);
                Mvx.IoCProvider.RegisterSingleton<IMvxSourceBindingFactoryExtensionHost>(extensionHost);
            }
            else
                MvxLog.Instance.Trace("source binding factory extension host not provided - so no source extensions will be used");
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
            var targetRegistry = CreateTargetBindingRegistry();
            Mvx.IoCProvider.RegisterSingleton<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            Mvx.IoCProvider.RegisterSingleton<IMvxTargetBindingFactory>(targetRegistry);
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