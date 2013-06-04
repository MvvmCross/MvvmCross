// MvxCoreBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.ExpressionParse;
using Cirrious.MvvmCross.Binding.Parse.Binding;
using Cirrious.MvvmCross.Binding.Parse.Binding.Lang;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath;
using Cirrious.MvvmCross.Localization;

namespace Cirrious.MvvmCross.Binding
{
    public class MvxCoreBindingBuilder
    {
        public virtual void DoRegistration()
        {
            CreateSingleton();
            RegisterCore();
            RegisterValueConverterRegistryFiller();
            RegisterValueConverterProvider();
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
            Mvx.RegisterSingleton<IMvxAutoValueConverters>(autoValueConverters);
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
            MvxBindingSingletonCache.Initialise();
        }

        protected virtual void RegisterValueConverterRegistryFiller()
        {
            Mvx.RegisterSingleton(CreateValueConverterRegistryFiller());
        }

        protected virtual IMvxValueConverterRegistryFiller CreateValueConverterRegistryFiller()
        {
            return new MvxValueConverterRegistryFiller();
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
            var registry = new MvxValueConverterRegistry();
            Mvx.RegisterSingleton<IMvxValueConverterLookup>(registry);
            Mvx.RegisterSingleton<IMvxValueConverterRegistry>(registry);
            FillValueConverters(registry);
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwriteFrom(GetType().Assembly);
            registry.AddOrOverwriteFrom(typeof(MvxLanguageConverter).Assembly);
        }

        protected virtual void RegisterBindingParser()
        {
            if (Mvx.CanResolve<IMvxBindingParser>())
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic,
                                      "Binding Parser already registered - so skipping Swiss parser");
                return;
            }
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Registering Swiss Binding Parser");
            Mvx.RegisterSingleton(CreateBindingParser());
        }

        protected virtual IMvxBindingParser CreateBindingParser()
        {
            return new MvxSwissBindingParser();
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
            Mvx.RegisterSingleton(CreateLanguageBindingParser());
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
            var tokeniser = CreateSourcePropertyPathParser();
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