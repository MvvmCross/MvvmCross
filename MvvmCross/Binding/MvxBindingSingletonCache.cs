// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Base;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Binding.ExpressionParse;
using MvvmCross.Binding.Parse.Binding.Lang;
using MvvmCross.Hosting;

namespace MvvmCross.Binding
{
    // this class is not perfect OO and it gets in the way of testing
    // however, it is here for speed - to help avoid obscene numbers of service location calls during binding
    public class MvxBindingSingletonCache : IMvxBindingSingletonCache
    {
        /// <summary>
        /// Resolves the registered <see cref="IMvxBindingSingletonCache"/> from the ambient host.
        /// Returns <c>null</c> if the host has not been started yet.
        /// </summary>
        public static IMvxBindingSingletonCache Instance =>
            MvxHost.Current?.Services.GetService<IMvxBindingSingletonCache>();

        private IMvxAutoValueConverters _autoValueConverters;
        private IMvxBindingDescriptionParser _bindingDescriptionParser;
        private IMvxSourceBindingFactory _sourceBindingFactory;
        private IMvxTargetBindingFactory _targetBindingFactory;
        private IMvxLanguageBindingParser _languageParser;
        private IMvxPropertyExpressionParser _propertyExpressionParser;
        private IMvxValueConverterLookup _valueConverterLookup;
        private IMvxBindingNameLookup _defaultBindingName;
        private IMvxBinder _binder;
        private IMvxSourceStepFactory _sourceStepFactory;
        private IMvxValueCombinerLookup _valueCombinerLookup;
        private IMvxMainThreadAsyncDispatcher _mainThreadDispatcher;

        public IMvxAutoValueConverters AutoValueConverters
        {
            get
            {
                _autoValueConverters ??= MvxHost.Current?.Services.GetService<IMvxAutoValueConverters>();
                return _autoValueConverters;
            }
        }

        public IMvxBindingDescriptionParser BindingDescriptionParser
        {
            get
            {
                _bindingDescriptionParser ??= MvxHost.Current?.Services.GetService<IMvxBindingDescriptionParser>();
                return _bindingDescriptionParser;
            }
        }

        public IMvxLanguageBindingParser LanguageParser
        {
            get
            {
                _languageParser ??= MvxHost.Current?.Services.GetService<IMvxLanguageBindingParser>();
                return _languageParser;
            }
        }

        public IMvxPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                _propertyExpressionParser ??= MvxHost.Current?.Services.GetService<IMvxPropertyExpressionParser>();
                return _propertyExpressionParser;
            }
        }

        public IMvxValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup ??= MvxHost.Current?.Services.GetService<IMvxValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        public IMvxValueCombinerLookup ValueCombinerLookup
        {
            get
            {
                _valueCombinerLookup ??= MvxHost.Current?.Services.GetService<IMvxValueCombinerLookup>();
                return _valueCombinerLookup;
            }
        }

        public IMvxBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                _defaultBindingName ??= MvxHost.Current?.Services.GetService<IMvxBindingNameLookup>();
                return _defaultBindingName;
            }
        }

        public IMvxBinder Binder
        {
            get
            {
                _binder ??= MvxHost.Current?.Services.GetService<IMvxBinder>();
                return _binder;
            }
        }

        public IMvxSourceBindingFactory SourceBindingFactory
        {
            get
            {
                _sourceBindingFactory ??= MvxHost.Current?.Services.GetService<IMvxSourceBindingFactory>();
                return _sourceBindingFactory;
            }
        }

        public IMvxTargetBindingFactory TargetBindingFactory
        {
            get
            {
                _targetBindingFactory ??= MvxHost.Current?.Services.GetService<IMvxTargetBindingFactory>();
                return _targetBindingFactory;
            }
        }

        public IMvxSourceStepFactory SourceStepFactory
        {
            get
            {
                _sourceStepFactory ??= MvxHost.Current?.Services.GetService<IMvxSourceStepFactory>();
                return _sourceStepFactory;
            }
        }

        public IMvxMainThreadAsyncDispatcher MainThreadDispatcher
        {
            get
            {
                _mainThreadDispatcher ??= MvxHost.Current?.Services.GetService<IMvxMainThreadAsyncDispatcher>();
                return _mainThreadDispatcher;
            }
        }
    }
}
