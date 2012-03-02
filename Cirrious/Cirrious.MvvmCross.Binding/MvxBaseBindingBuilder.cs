#region Copyright
// <copyright file="MvxBaseBindingBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Binders.Json;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding
{
    public class MvxBaseBindingBuilder
        : IMvxServiceProducer<IMvxTargetBindingFactoryRegistry>
          , IMvxServiceProducer<IMvxTargetBindingFactory>        
          , IMvxServiceProducer<IMvxSourceBindingFactory>
          , IMvxServiceProducer<IMvxValueConverterRegistry>
          , IMvxServiceProducer<IMvxValueConverterProvider>
          , IMvxServiceProducer<IMvxBinder>
          , IMvxServiceProducer<IMvxBindingDescriptionParser>
    {
        public virtual void DoRegistration()
        {
            RegisterCore();
            RegisterSourceFactory();
            RegisterTargetFactory();
            RegisterValueConverterProvider();
            RegisterBindingParametersParser();
            RegisterPlatformSpecificComponents();
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

        protected virtual void RegisterBindingParametersParser()
        {
            this.RegisterServiceInstance<IMvxBindingDescriptionParser>(new MvxJsonBindingDescriptionParser());
        }

        protected virtual void RegisterPlatformSpecificComponents()
        {
            // nothing to do here            
        }
    }
}