// MvxBindingSingletonCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding
{
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings.Source.Construction;
    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Binding.Combiners;
    using MvvmCross.Binding.ExpressionParse;
    using MvvmCross.Binding.Parse.Binding.Lang;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;

    // this class is not perfect OO and it gets in the way of testing
    // however, it is here for speed - to help avoid obscene numbers of Mvx.Resolve<T> calls during binding

    public class MvxBindingSingletonCache
        : MvxSingleton<IMvxBindingSingletonCache>
          , IMvxBindingSingletonCache
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

        public IMvxAutoValueConverters AutoValueConverters
        {
            get
            {
                this._autoValueConverters = this._autoValueConverters ?? Mvx.Resolve<IMvxAutoValueConverters>();
                return this._autoValueConverters;
            }
        }

        public IMvxBindingDescriptionParser BindingDescriptionParser
        {
            get
            {
                this._bindingDescriptionParser = this._bindingDescriptionParser ?? Mvx.Resolve<IMvxBindingDescriptionParser>();
                return this._bindingDescriptionParser;
            }
        }

        public IMvxLanguageBindingParser LanguageParser
        {
            get
            {
                this._languageParser = this._languageParser ?? Mvx.Resolve<IMvxLanguageBindingParser>();
                return this._languageParser;
            }
        }

        public IMvxPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                this._propertyExpressionParser = this._propertyExpressionParser ?? Mvx.Resolve<IMvxPropertyExpressionParser>();
                return this._propertyExpressionParser;
            }
        }

        public IMvxValueConverterLookup ValueConverterLookup
        {
            get
            {
                this._valueConverterLookup = this._valueConverterLookup ?? Mvx.Resolve<IMvxValueConverterLookup>();
                return this._valueConverterLookup;
            }
        }

        public IMvxValueCombinerLookup ValueCombinerLookup
        {
            get
            {
                this._valueCombinerLookup = this._valueCombinerLookup ?? Mvx.Resolve<IMvxValueCombinerLookup>();
                return this._valueCombinerLookup;
            }
        }

        public IMvxBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                this._defaultBindingName = this._defaultBindingName ?? Mvx.Resolve<IMvxBindingNameLookup>();
                return this._defaultBindingName;
            }
        }

        public IMvxBinder Binder
        {
            get
            {
                this._binder = this._binder ?? Mvx.Resolve<IMvxBinder>();
                return this._binder;
            }
        }

        public IMvxSourceBindingFactory SourceBindingFactory
        {
            get
            {
                this._sourceBindingFactory = this._sourceBindingFactory ?? Mvx.Resolve<IMvxSourceBindingFactory>();
                return this._sourceBindingFactory;
            }
        }

        public IMvxTargetBindingFactory TargetBindingFactory
        {
            get
            {
                this._targetBindingFactory = this._targetBindingFactory ?? Mvx.Resolve<IMvxTargetBindingFactory>();
                return this._targetBindingFactory;
            }
        }

        public IMvxSourceStepFactory SourceStepFactory
        {
            get
            {
                this._sourceStepFactory = this._sourceStepFactory ?? Mvx.Resolve<IMvxSourceStepFactory>();
                return this._sourceStepFactory;
            }
        }
    }
}