// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Binding.ExpressionParse;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Binding.Parse.Binding.Lang;
using MvvmCross.Binding.Parse.Binding.Tibet;
using MvvmCross.Binding.Parse.PropertyPath;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Converters;
using MvvmCross.Localization;

namespace MvvmCross.Binding
{
    public class MvxCoreBindingBuilder
    {
        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        public virtual void DoRegistration(IServiceCollection services)
        {
            CreateSingleton(services);
            RegisterCore(services);
            RegisterValueConverterRegistryFiller(services);
            RegisterValueConverterProvider(services);
            RegisterValueCombinerRegistryFiller(services);
            RegisterValueCombinerProvider(services);
            RegisterAutoValueConverters(services);
            RegisterBindingParser(services);
            RegisterLanguageBindingParser(services);
            RegisterBindingDescriptionParser(services);
            RegisterExpressionParser(services);
            RegisterSourcePropertyPathParser(services);
            RegisterPlatformSpecificComponents(services);
            RegisterBindingNameRegistry(services);
        }

        protected virtual void RegisterAutoValueConverters(IServiceCollection services)
        {
            var autoValueConverters = CreateAutoValueConverters();
            services.TryAddSingleton<IMvxAutoValueConverters>(autoValueConverters);
            FillAutoValueConverters(autoValueConverters);
        }

        protected virtual void FillAutoValueConverters(IMvxAutoValueConverters autoValueConverters)
        {
            // nothing to do in base class
        }

        protected virtual IMvxAutoValueConverters CreateAutoValueConverters()
        {
            return new MvxAutoValueConverters();
        }

        protected virtual void CreateSingleton(IServiceCollection services)
        {
            services.TryAddSingleton<IMvxBindingSingletonCache, MvxBindingSingletonCache>();
        }

        protected virtual void RegisterValueConverterRegistryFiller(IServiceCollection services)
        {
            var filler = CreateValueConverterRegistryFiller();
            services.TryAddSingleton<IMvxNamedInstanceRegistryFiller<IMvxValueConverter>>(filler);
            services.TryAddSingleton<IMvxValueConverterRegistryFiller>(filler);
        }

        protected virtual IMvxValueConverterRegistryFiller CreateValueConverterRegistryFiller()
        {
            return new MvxValueConverterRegistryFiller();
        }

        protected virtual void RegisterValueCombinerRegistryFiller(IServiceCollection services)
        {
            var filler = CreateValueCombinerRegistryFiller();
            services.TryAddSingleton<IMvxNamedInstanceRegistryFiller<IMvxValueCombiner>>(filler);
            services.TryAddSingleton<IMvxValueCombinerRegistryFiller>(filler);
        }

        protected virtual IMvxValueCombinerRegistryFiller CreateValueCombinerRegistryFiller()
        {
            return new MvxValueCombinerRegistryFiller();
        }

        protected virtual void RegisterExpressionParser(IServiceCollection services)
        {
            services.TryAddTransient<IMvxPropertyExpressionParser, MvxPropertyExpressionParser>();
        }

        protected virtual void RegisterCore(IServiceCollection services)
        {
            services.TryAddSingleton<IMvxBinder>(new MvxFromTextBinder());
            services.TryAddTransient<IMvxBindingContext, MvxTaskBasedBindingContext>();
        }

        protected virtual void RegisterValueConverterProvider(IServiceCollection services)
        {
            // Register the value converter registry as a DI-factory singleton so that
            // IConfigureMvxValueConverters implementations (registered by plugins) are
            // applied when the registry is first resolved from the container.
            services.TryAddSingleton<IMvxValueConverterRegistry>(sp =>
            {
                var registry = CreateValueConverterRegistry();
                FillValueConverters(registry);
                foreach (var configurator in sp.GetServices<IConfigureMvxValueConverters>())
                    configurator.Register(registry);
                return registry;
            });
            services.TryAddSingleton<IMvxNamedInstanceLookup<IMvxValueConverter>>(
                sp => (IMvxNamedInstanceLookup<IMvxValueConverter>)sp.GetRequiredService<IMvxValueConverterRegistry>());
            services.TryAddSingleton<IMvxNamedInstanceRegistry<IMvxValueConverter>>(
                sp => (IMvxNamedInstanceRegistry<IMvxValueConverter>)sp.GetRequiredService<IMvxValueConverterRegistry>());
            services.TryAddSingleton<IMvxValueConverterLookup>(
                sp => sp.GetRequiredService<IMvxValueConverterRegistry>());
        }

        protected virtual MvxValueConverterRegistry CreateValueConverterRegistry()
        {
            return new MvxValueConverterRegistry();
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("CommandParameter", new MvxCommandParameterValueConverter());
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }

        protected virtual void RegisterValueCombinerProvider(IServiceCollection services)
        {
            var registry = CreateValueCombinerRegistry();
            services.TryAddSingleton<IMvxNamedInstanceLookup<IMvxValueCombiner>>(registry);
            services.TryAddSingleton<IMvxNamedInstanceRegistry<IMvxValueCombiner>>(registry);
            services.TryAddSingleton<IMvxValueCombinerLookup>(registry);
            services.TryAddSingleton<IMvxValueCombinerRegistry>(registry);
            FillValueCombiners(registry);
        }

        protected virtual IMvxValueCombinerRegistry CreateValueCombinerRegistry()
        {
            return new MvxValueCombinerRegistry();
        }

        protected virtual void FillValueCombiners(IMvxValueCombinerRegistry registry)
        {
            // note that assembly based registration is not used here for efficiency reasons
            // - see #327 - https://github.com/slodge/MvvmCross/issues/327
            registry.AddOrOverwrite("Add", new MvxAddValueCombiner());
            registry.AddOrOverwrite("Divide", new MvxDivideValueCombiner());
            registry.AddOrOverwrite("Format", new MvxFormatValueCombiner());
            registry.AddOrOverwrite("If", new MvxIfValueCombiner());
            registry.AddOrOverwrite("Modulus", new MvxModulusValueCombiner());
            registry.AddOrOverwrite("Multiply", new MvxMultiplyValueCombiner());
            registry.AddOrOverwrite("Single", new MvxSingleValueCombiner());
            registry.AddOrOverwrite("Subtract", new MvxSubtractValueCombiner());
            registry.AddOrOverwrite("EqualTo", new MvxEqualToValueCombiner());
            registry.AddOrOverwrite("NotEqualTo", new MvxNotEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThanOrEqualTo", new MvxGreaterThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThan", new MvxGreaterThanValueCombiner());
            registry.AddOrOverwrite("LessThanOrEqualTo", new MvxLessThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("LessThan", new MvxLessThanValueCombiner());
            registry.AddOrOverwrite("Not", new MvxNotValueCombiner());
            registry.AddOrOverwrite("And", new MvxAndValueCombiner());
            registry.AddOrOverwrite("Or", new MvxOrValueCombiner());
            registry.AddOrOverwrite("XOr", new MvxXorValueCombiner());
            registry.AddOrOverwrite("Inverted", new MvxInvertedValueCombiner());

            // Note: MvxValueConverterValueCombiner is not registered - it is unconventional
            //registry.AddOrOverwrite("ValueConverter", new MvxValueConverterValueCombiner());
        }

        protected virtual void RegisterBindingParser(IServiceCollection services)
        {
            if (services.Any(sd => sd.ServiceType == typeof(IMvxBindingParser)))
            {
                MvxBindingLog.Instance?.LogTrace("Binding Parser already registered - so skipping Default parser");
                return;
            }
            MvxBindingLog.Instance?.LogTrace("Registering Default Binding Parser");
            services.TryAddSingleton<IMvxBindingParser>(CreateBindingParser());
        }

        protected virtual IMvxBindingParser CreateBindingParser()
        {
            return new MvxTibetBindingParser();
        }

        protected virtual void RegisterLanguageBindingParser(IServiceCollection services)
        {
            if (services.Any(sd => sd.ServiceType == typeof(IMvxLanguageBindingParser)))
            {
                MvxBindingLog.Instance?.LogTrace("Binding Parser already registered - so skipping Language parser");
                return;
            }
            MvxBindingLog.Instance?.LogTrace("Registering Language Binding Parser");
            services.TryAddSingleton<IMvxLanguageBindingParser>(CreateLanguageBindingParser());
        }

        protected virtual IMvxLanguageBindingParser CreateLanguageBindingParser()
        {
            return new MvxLanguageBindingParser();
        }

        protected virtual void RegisterBindingDescriptionParser(IServiceCollection services)
        {
            var parser = CreateBindingDescriptionParser();
            services.TryAddSingleton<IMvxBindingDescriptionParser>(parser);
        }

        private static IMvxBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new MvxBindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourcePropertyPathParser(IServiceCollection services)
        {
            var tokeniser = CreateSourcePropertyPathParser();
            services.TryAddSingleton<IMvxSourcePropertyPathParser>(tokeniser);
        }

        protected virtual IMvxSourcePropertyPathParser CreateSourcePropertyPathParser()
        {
            return new MvxSourcePropertyPathParser();
        }

        protected virtual void RegisterBindingNameRegistry(IServiceCollection services)
        {
            var registry = new MvxBindingNameRegistry();
            services.TryAddSingleton<IMvxBindingNameLookup>(registry);
            services.TryAddSingleton<IMvxBindingNameRegistry>(registry);
            FillDefaultBindingNames(registry);
        }

        protected virtual void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            // base class has nothing to register
        }

        protected virtual void RegisterPlatformSpecificComponents(IServiceCollection services)
        {
            // nothing to do here
        }

    }
}
