// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
using MvvmCross.Logging;

namespace MvvmCross.Binding
{
    public class MvxBindingBuilder : MvxCoreBindingBuilder
    {
        public override void DoRegistration(IMvxIoCProvider iocProvider)
        {
            base.DoRegistration(iocProvider);
            RegisterBindingFactories(iocProvider);
        }

        protected virtual void RegisterBindingFactories(IMvxIoCProvider iocProvider)
        {
            RegisterMvxBindingFactories(iocProvider);
        }

        protected virtual void RegisterMvxBindingFactories(IMvxIoCProvider iocProvider)
        {
            RegisterSourceStepFactory(iocProvider);
            RegisterSourceFactory(iocProvider);
            RegisterTargetFactory(iocProvider);
        }

        protected virtual void RegisterSourceStepFactory(IMvxIoCProvider iocProvider)
        {
            var sourceStepFactory = CreateSourceStepFactoryRegistry();
            FillSourceStepFactory(sourceStepFactory);
            iocProvider.RegisterSingleton<IMvxSourceStepFactoryRegistry>(sourceStepFactory);
            iocProvider.RegisterSingleton<IMvxSourceStepFactory>(sourceStepFactory);
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

        protected virtual void RegisterSourceFactory(IMvxIoCProvider iocProvider)
        {
            var sourceFactory = CreateSourceBindingFactory();
            iocProvider.RegisterSingleton<IMvxSourceBindingFactory>(sourceFactory);
            var extensionHost = sourceFactory as IMvxSourceBindingFactoryExtensionHost;
            if (extensionHost != null)
            {
                RegisterSourceBindingFactoryExtensions(extensionHost);
                iocProvider.RegisterSingleton<IMvxSourceBindingFactoryExtensionHost>(extensionHost);
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

        protected virtual void RegisterTargetFactory(IMvxIoCProvider iocProvider)
        {
            var targetRegistry = CreateTargetBindingRegistry();
            FillTargetFactories(targetRegistry);
            iocProvider.RegisterSingleton<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            iocProvider.RegisterSingleton<IMvxTargetBindingFactory>(targetRegistry);
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
