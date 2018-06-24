// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Binding.ExpressionParse;
using MvvmCross.Binding.Parse.Binding.Lang;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding
{
    // this class is not perfect OO and it gets in the way of testing
    // however, it is here for speed - to help avoid obscene numbers of Mvx.IoCProvider.Resolve<T> calls during binding
    public class MvxBindingSingletonCache
        : MvxSingleton<IMvxBindingSingletonCache>, IMvxBindingSingletonCache
    {
        public static IMvxBindingSingletonCache Initialize()
        {
            if (Instance != null)
                throw new MvxException("You should only initialize MvxBindingSingletonCache once");

            var instance = new MvxBindingSingletonCache();
            return instance;
        }

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
        private IMvxMainThreadDispatcher _mainThreadDispatcher;

        public IMvxAutoValueConverters AutoValueConverters
        {
            get
            {
                _autoValueConverters = _autoValueConverters ?? Mvx.IoCProvider.Resolve<IMvxAutoValueConverters>();
                return _autoValueConverters;
            }
        }

        public IMvxBindingDescriptionParser BindingDescriptionParser
        {
            get
            {
                _bindingDescriptionParser = _bindingDescriptionParser ?? Mvx.IoCProvider.Resolve<IMvxBindingDescriptionParser>();
                return _bindingDescriptionParser;
            }
        }

        public IMvxLanguageBindingParser LanguageParser
        {
            get
            {
                _languageParser = _languageParser ?? Mvx.IoCProvider.Resolve<IMvxLanguageBindingParser>();
                return _languageParser;
            }
        }

        public IMvxPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                _propertyExpressionParser = _propertyExpressionParser ?? Mvx.IoCProvider.Resolve<IMvxPropertyExpressionParser>();
                return _propertyExpressionParser;
            }
        }

        public IMvxValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? Mvx.IoCProvider.Resolve<IMvxValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        public IMvxValueCombinerLookup ValueCombinerLookup
        {
            get
            {
                _valueCombinerLookup = _valueCombinerLookup ?? Mvx.IoCProvider.Resolve<IMvxValueCombinerLookup>();
                return _valueCombinerLookup;
            }
        }

        public IMvxBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                _defaultBindingName = _defaultBindingName ?? Mvx.IoCProvider.Resolve<IMvxBindingNameLookup>();
                return _defaultBindingName;
            }
        }

        public IMvxBinder Binder
        {
            get
            {
                _binder = _binder ?? Mvx.IoCProvider.Resolve<IMvxBinder>();
                return _binder;
            }
        }

        public IMvxSourceBindingFactory SourceBindingFactory
        {
            get
            {
                _sourceBindingFactory = _sourceBindingFactory ?? Mvx.IoCProvider.Resolve<IMvxSourceBindingFactory>();
                return _sourceBindingFactory;
            }
        }

        public IMvxTargetBindingFactory TargetBindingFactory
        {
            get
            {
                _targetBindingFactory = _targetBindingFactory ?? Mvx.IoCProvider.Resolve<IMvxTargetBindingFactory>();
                return _targetBindingFactory;
            }
        }

        public IMvxSourceStepFactory SourceStepFactory
        {
            get
            {
                _sourceStepFactory = _sourceStepFactory ?? Mvx.IoCProvider.Resolve<IMvxSourceStepFactory>();
                return _sourceStepFactory;
            }
        }

        public IMvxMainThreadDispatcher MainThreadDispatcher
        {
            get
            {
                _mainThreadDispatcher = _mainThreadDispatcher ?? Mvx.IoCProvider.Resolve<IMvxMainThreadDispatcher>();
                return _mainThreadDispatcher;
            }
        }
    }
}