// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Logging;

namespace MvvmCross.Binding
{
    public class MvxBindingBuilder : MvxCoreBindingBuilder
    {
        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        public override void DoRegistration(IServiceCollection services)
        {
            base.DoRegistration(services);
            RegisterBindingFactories(services);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected virtual void RegisterBindingFactories(IServiceCollection services)
        {
            RegisterMvxBindingFactories(services);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected virtual void RegisterMvxBindingFactories(IServiceCollection services)
        {
            RegisterSourceStepFactory(services);
            RegisterSourceFactory(services);
            RegisterTargetFactory(services);
        }

        protected virtual void RegisterSourceStepFactory(IServiceCollection services)
        {
            var sourceStepFactory = CreateSourceStepFactoryRegistry();
            FillSourceStepFactory(sourceStepFactory);
            services.TryAddSingleton<IMvxSourceStepFactoryRegistry>(sourceStepFactory);
            services.TryAddSingleton<IMvxSourceStepFactory>(sourceStepFactory);
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

        protected virtual void RegisterSourceFactory(IServiceCollection services)
        {
            var sourceFactory = CreateSourceBindingFactory();
            services.TryAddSingleton<IMvxSourceBindingFactory>(sourceFactory);
            var extensionHost = sourceFactory as IMvxSourceBindingFactoryExtensionHost;
            if (extensionHost != null)
            {
                RegisterSourceBindingFactoryExtensions(extensionHost);
                services.TryAddSingleton<IMvxSourceBindingFactoryExtensionHost>(extensionHost);
            }
            else
                MvxLogHost.Default?.Log(LogLevel.Trace, "source binding factory extension host not provided - so no source extensions will be used");
        }

        protected virtual void RegisterSourceBindingFactoryExtensions(IMvxSourceBindingFactoryExtensionHost extensionHost)
        {
            extensionHost.Extensions.Add(new MvxPropertySourceBindingFactoryExtension());
        }

        protected virtual IMvxSourceBindingFactory CreateSourceBindingFactory()
        {
            return new MvxSourceBindingFactory();
        }

        [RequiresUnreferencedCode("This method registers target bindings that may not be preserved by trimming")]
        protected virtual void RegisterTargetFactory(IServiceCollection services)
        {
            var targetRegistry = CreateTargetBindingRegistry();
            FillTargetFactories(targetRegistry);
            services.TryAddSingleton<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            services.TryAddSingleton<IMvxTargetBindingFactory>(targetRegistry);
        }

        protected virtual IMvxTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            return new MvxTargetBindingFactoryRegistry();
        }

        [RequiresUnreferencedCode("This method registers target bindings that may not be preserved by trimming")]
        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // base class has nothing to register
        }

    }
}
