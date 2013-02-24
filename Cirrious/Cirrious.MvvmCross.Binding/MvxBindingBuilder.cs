// MvxBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.Binding.Parse.Binding;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath;

namespace Cirrious.MvvmCross.Binding
{
    public class MvxBindingBuilder
        : IMvxProducer
          , IMvxConsumer
    {
        public virtual void DoRegistration()
        {
            RegisterCore();
            RegisterSourceFactory();
            RegisterTargetFactory();
            RegisterValueConverterProvider();
            RegisterBindingParser();
            RegisterBindingDescriptionParser();
            RegisterPlatformSpecificComponents();
            RegisterSourceBindingTokeniser();
        }

        protected virtual void RegisterCore()
        {
            var binder = new MvxFromTextBinder();
            this.RegisterSingleton<IMvxBinder>(binder);
        }

        protected virtual void RegisterSourceFactory()
        {
            var sourceFactory = new MvxSourceBindingFactory();
            this.RegisterSingleton<IMvxSourceBindingFactory>(sourceFactory);
        }

        protected virtual void RegisterTargetFactory()
        {
            var targetRegistry = new MvxTargetBindingFactoryRegistry();
            this.RegisterSingleton<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            this.RegisterSingleton<IMvxTargetBindingFactory>(targetRegistry);
            FillTargetFactories(targetRegistry);
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // base class has nothing to register
        }

        protected virtual void RegisterValueConverterProvider()
        {
            var registry = new MvxValueConverterRegistry();
            this.RegisterSingleton<IMvxValueConverterRegistry>(registry);
            this.RegisterSingleton<IMvxValueConverterProvider>(registry);
            FillValueConverters(registry);
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            // nothing to do here            
        }

        protected virtual void RegisterBindingParser()
        {
            if (this.CanResolve<IMvxBindingParser>())
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic,
                                      "Binding Parser already registered - so skipping Swiss parser");
                return;
            }
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Registering Swiss Binding Parser");
            this.RegisterSingleton<IMvxBindingParser>(new MvxSwissBindingParser());
        }

        protected virtual void RegisterBindingDescriptionParser()
        {
            var parser = CreateBindingDescriptionParser();
            this.RegisterSingleton(parser);
        }

        private static IMvxBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new MvxBindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourceBindingTokeniser()
        {
            var tokeniser = new MvxSourcePropertyPathParser();
            this.RegisterSingleton<IMvxSourcePropertyPathParser>(tokeniser);
        }

        protected virtual void RegisterPlatformSpecificComponents()
        {
            // nothing to do here            
        }
    }
}