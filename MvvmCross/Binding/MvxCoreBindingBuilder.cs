// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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
        public virtual void DoRegistration()
        {
            CreateSingleton();
            RegisterCore();
            RegisterValueConverterRegistryFiller();
            RegisterValueConverterProvider();
            RegisterValueCombinerRegistryFiller();
            RegisterValueCombinerProvider();
            RegisterAutoValueConverters();
            RegisterBindingParser();
            RegisterLanguageBindingParser();
            RegisterBindingDescriptionParser();
            RegisterExpressionParser();
            RegisterSourcePropertyPathParser();
            RegisterPlatformSpecificComponents();
            RegisterBindingNameRegistry();
        }

        protected virtual void RegisterAutoValueConverters()
        {
            var autoValueConverters = CreateAutoValueConverters();
            Mvx.IoCProvider.RegisterSingleton<IMvxAutoValueConverters>(autoValueConverters);
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

        protected virtual void CreateSingleton()
        {
            MvxBindingSingletonCache.Initialize();
        }

        protected virtual void RegisterValueConverterRegistryFiller()
        {
            var filler = CreateValueConverterRegistryFiller();
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceRegistryFiller<IMvxValueConverter>>(filler);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueConverterRegistryFiller>(filler);
        }

        protected virtual IMvxValueConverterRegistryFiller CreateValueConverterRegistryFiller()
        {
            return new MvxValueConverterRegistryFiller();
        }

        protected virtual void RegisterValueCombinerRegistryFiller()
        {
            var filler = CreateValueCombinerRegistryFiller();
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceRegistryFiller<IMvxValueCombiner>>(filler);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueCombinerRegistryFiller>(filler);
        }

        protected virtual IMvxValueCombinerRegistryFiller CreateValueCombinerRegistryFiller()
        {
            return new MvxValueCombinerRegistryFiller();
        }

        protected virtual void RegisterExpressionParser()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxPropertyExpressionParser>(new MvxPropertyExpressionParser());
        }

        protected virtual void RegisterCore()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxBinder>(new MvxFromTextBinder());

            //To get the old behavior back, you can override this registration with
            //Mvx.IoCProvider.RegisterType<IMvxBindingContext, MvxBindingContext>();
            Mvx.IoCProvider.RegisterType<IMvxBindingContext, MvxTaskBasedBindingContext>();
        }

        protected virtual void RegisterValueConverterProvider()
        {
            var registry = CreateValueConverterRegistry();
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceLookup<IMvxValueConverter>>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceRegistry<IMvxValueConverter>>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueConverterLookup>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueConverterRegistry>(registry);
            FillValueConverters(registry);
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

        protected virtual void RegisterValueCombinerProvider()
        {
            var registry = CreateValueCombinerRegistry();
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceLookup<IMvxValueCombiner>>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceRegistry<IMvxValueCombiner>>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueCombinerLookup>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueCombinerRegistry>(registry);
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

        protected virtual void RegisterBindingParser()
        {
            if (Mvx.IoCProvider.CanResolve<IMvxBindingParser>())
            {
                MvxBindingLog.Trace("Binding Parser already registered - so skipping Default parser");
                return;
            }
            MvxBindingLog.Trace("Registering Default Binding Parser");
            Mvx.IoCProvider.RegisterSingleton(CreateBindingParser());
        }

        protected virtual IMvxBindingParser CreateBindingParser()
        {
            return new MvxTibetBindingParser();
        }

        protected virtual void RegisterLanguageBindingParser()
        {
            if (Mvx.IoCProvider.CanResolve<IMvxLanguageBindingParser>())
            {
                MvxBindingLog.Trace("Binding Parser already registered - so skipping Language parser");
                return;
            }
            MvxBindingLog.Trace("Registering Language Binding Parser");
            Mvx.IoCProvider.RegisterSingleton(CreateLanguageBindingParser());
        }

        protected virtual IMvxLanguageBindingParser CreateLanguageBindingParser()
        {
            return new MvxLanguageBindingParser();
        }

        protected virtual void RegisterBindingDescriptionParser()
        {
            var parser = CreateBindingDescriptionParser();
            Mvx.IoCProvider.RegisterSingleton(parser);
        }

        private static IMvxBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new MvxBindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourcePropertyPathParser()
        {
            var tokeniser = CreateSourcePropertyPathParser();
            Mvx.IoCProvider.RegisterSingleton<IMvxSourcePropertyPathParser>(tokeniser);
        }

        protected virtual IMvxSourcePropertyPathParser CreateSourcePropertyPathParser()
        {
            return new MvxSourcePropertyPathParser();
        }

        protected virtual void RegisterBindingNameRegistry()
        {
            var registry = new MvxBindingNameRegistry();
            Mvx.IoCProvider.RegisterSingleton<IMvxBindingNameLookup>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxBindingNameRegistry>(registry);
            FillDefaultBindingNames(registry);
        }

        protected virtual void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            // base class has nothing to register
        }

        protected virtual void RegisterPlatformSpecificComponents()
        {
            // nothing to do here
        }
    }
}
