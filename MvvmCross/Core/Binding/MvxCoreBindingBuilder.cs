// MvxCoreBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding
{
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Combiners;
    using MvvmCross.Binding.Combiners.VeryExperimental;
    using MvvmCross.Binding.ExpressionParse;
    using MvvmCross.Binding.Parse.Binding;
    using MvvmCross.Binding.Parse.Binding.Lang;
    using MvvmCross.Binding.Parse.Binding.Tibet;
    using MvvmCross.Binding.Parse.PropertyPath;
    using MvvmCross.Binding.ValueConverters;
    using MvvmCross.Localization;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Platform;

    public class MvxCoreBindingBuilder
    {
        public virtual void DoRegistration()
        {
            this.CreateSingleton();
            this.RegisterCore();
            this.RegisterValueConverterRegistryFiller();
            this.RegisterValueConverterProvider();
            this.RegisterValueCombinerRegistryFiller();
            this.RegisterValueCombinerProvider();
            this.RegisterAutoValueConverters();
            this.RegisterBindingParser();
            this.RegisterLanguageBindingParser();
            this.RegisterBindingDescriptionParser();
            this.RegisterExpressionParser();
            this.RegisterSourcePropertyPathParser();
            this.RegisterPlatformSpecificComponents();
            this.RegisterBindingNameRegistry();
        }

        protected virtual void RegisterAutoValueConverters()
        {
            var autoValueConverters = this.CreateAutoValueConverters();
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(autoValueConverters);
            this.FillAutoValueConverters(autoValueConverters);
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
            var filler = this.CreateValueConverterRegistryFiller();
            Mvx.RegisterSingleton<IMvxNamedInstanceRegistryFiller<IMvxValueConverter>>(filler);
            Mvx.RegisterSingleton<IMvxValueConverterRegistryFiller>(filler);
        }

        protected virtual IMvxValueConverterRegistryFiller CreateValueConverterRegistryFiller()
        {
            return new MvxValueConverterRegistryFiller();
        }

        protected virtual void RegisterValueCombinerRegistryFiller()
        {
            var filler = this.CreateValueCombinerRegistryFiller();
            Mvx.RegisterSingleton<IMvxNamedInstanceRegistryFiller<IMvxValueCombiner>>(filler);
            Mvx.RegisterSingleton<IMvxValueCombinerRegistryFiller>(filler);
        }

        protected virtual IMvxValueCombinerRegistryFiller CreateValueCombinerRegistryFiller()
        {
            return new MvxValueCombinerRegistryFiller();
        }

        protected virtual void RegisterExpressionParser()
        {
            Mvx.RegisterSingleton<IMvxPropertyExpressionParser>(new MvxPropertyExpressionParser());
        }

        protected virtual void RegisterCore()
        {
            var binder = new MvxFromTextBinder();
            Mvx.RegisterSingleton<IMvxBinder>(binder);
        }

        protected virtual void RegisterValueConverterProvider()
        {
            var registry = this.CreateValueConverterRegistry();
            Mvx.RegisterSingleton<IMvxNamedInstanceLookup<IMvxValueConverter>>(registry);
            Mvx.RegisterSingleton<IMvxNamedInstanceRegistry<IMvxValueConverter>>(registry);
            Mvx.RegisterSingleton<IMvxValueConverterLookup>(registry);
            Mvx.RegisterSingleton<IMvxValueConverterRegistry>(registry);
            this.FillValueConverters(registry);
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
            var registry = this.CreateValueCombinerRegistry();
            Mvx.RegisterSingleton<IMvxNamedInstanceLookup<IMvxValueCombiner>>(registry);
            Mvx.RegisterSingleton<IMvxNamedInstanceRegistry<IMvxValueCombiner>>(registry);
            Mvx.RegisterSingleton<IMvxValueCombinerLookup>(registry);
            Mvx.RegisterSingleton<IMvxValueCombinerRegistry>(registry);
            this.FillValueCombiners(registry);
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

            // Note: MvxValueConverterValueCombiner is not registered - it is unconventional
            //registry.AddOrOverwrite("ValueConverter", new MvxValueConverterValueCombiner());
        }

        protected virtual void RegisterBindingParser()
        {
            if (Mvx.CanResolve<IMvxBindingParser>())
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic,
                                      "Binding Parser already registered - so skipping Default parser");
                return;
            }
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Registering Default Binding Parser");
            Mvx.RegisterSingleton(this.CreateBindingParser());
        }

        protected virtual IMvxBindingParser CreateBindingParser()
        {
            return new MvxTibetBindingParser();
        }

        protected virtual void RegisterLanguageBindingParser()
        {
            if (Mvx.CanResolve<IMvxLanguageBindingParser>())
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic,
                                      "Binding Parser already registered - so skipping Language parser");
                return;
            }
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Registering Language Binding Parser");
            Mvx.RegisterSingleton(this.CreateLanguageBindingParser());
        }

        protected virtual IMvxLanguageBindingParser CreateLanguageBindingParser()
        {
            return new MvxLanguageBindingParser();
        }

        protected virtual void RegisterBindingDescriptionParser()
        {
            var parser = CreateBindingDescriptionParser();
            Mvx.RegisterSingleton(parser);
        }

        private static IMvxBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new MvxBindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourcePropertyPathParser()
        {
            var tokeniser = this.CreateSourcePropertyPathParser();
            Mvx.RegisterSingleton<IMvxSourcePropertyPathParser>(tokeniser);
        }

        protected virtual MvxSourcePropertyPathParser CreateSourcePropertyPathParser()
        {
            return new MvxSourcePropertyPathParser();
        }

        protected virtual void RegisterBindingNameRegistry()
        {
            var registry = new MvxBindingNameRegistry();
            Mvx.RegisterSingleton<IMvxBindingNameLookup>(registry);
            Mvx.RegisterSingleton<IMvxBindingNameRegistry>(registry);
            this.FillDefaultBindingNames(registry);
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