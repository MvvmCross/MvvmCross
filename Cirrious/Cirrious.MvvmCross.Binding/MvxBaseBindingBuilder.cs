// MvxBaseBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.Binding.Parse.Binding.Composite;
using Cirrious.MvvmCross.Binding.Parse.Binding.Json;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Parse.Binding;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding
{
    public class MvxBaseBindingBuilder
        : IMvxServiceProducer
        , IMvxServiceConsumer
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
            this.RegisterServiceInstance<IMvxBinder>(binder);
        }

        protected virtual void RegisterSourceFactory()
        {
            var sourceFactory = new MvxSourceBindingFactory();
            this.RegisterServiceInstance<IMvxSourceBindingFactory>(sourceFactory);
        }

        protected virtual void RegisterTargetFactory()
        {
            var targetRegistry = new MvxTargetBindingFactoryRegistry();
            this.RegisterServiceInstance<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            this.RegisterServiceInstance<IMvxTargetBindingFactory>(targetRegistry);
            FillTargetFactories(targetRegistry);
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // base class has nothing to register
        }

        protected virtual void RegisterValueConverterProvider()
        {
            var registry = new MvxValueConverterRegistry();
            this.RegisterServiceInstance<IMvxValueConverterRegistry>(registry);
            this.RegisterServiceInstance<IMvxValueConverterProvider>(registry);
            FillValueConverters(registry);
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            // nothing to do here            
        }

		protected virtual void RegisterBindingParser ()
		{
			if (this.IsServiceAvailable<IMvxBindingParser> ()) {
				MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Binding Parser already registered - so skipping Composite parser");
				return;
			}
			MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Registering Composite Binding Parser");
            this.RegisterServiceInstance<IMvxBindingParser>(new MvxCompositeBindingParser()); 
		}

        protected virtual void RegisterBindingDescriptionParser()
        {
            var parser = CreateBindingDescriptionParser();
            this.RegisterServiceInstance<IMvxBindingDescriptionParser>(parser);
        }

        private static IMvxBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new MvxBindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourceBindingTokeniser()
        {
            var tokeniser = new MvxSourcePropertyPathParser();
            this.RegisterServiceInstance<IMvxSourcePropertyPathParser>(tokeniser);
        }

        protected virtual void RegisterPlatformSpecificComponents()
        {
            // nothing to do here            
        }
    }
}