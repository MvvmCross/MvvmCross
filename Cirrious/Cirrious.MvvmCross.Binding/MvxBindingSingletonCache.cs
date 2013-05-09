﻿// MvxBindingSingletonCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.ExpressionParse;
using Cirrious.MvvmCross.Binding.Parse.Binding.Lang;

namespace Cirrious.MvvmCross.Binding
{
    // this class is not perfect OO and it gets in the way of testing
    // however, it is here for speed - to help avoid obscene numbers of Mvx.Resolve<T> calls during binding

    public class MvxBindingSingletonCache
        : MvxSingleton<IMvxBindingSingletonCache>
          , IMvxBindingSingletonCache
    {
        public static IMvxBindingSingletonCache Initialise()
        {
            if (Instance != null)
                throw new MvxException("You should only initialise MvxBindingSingletonCache once");

            var instance = new MvxBindingSingletonCache();
            return instance;
        }

        private IMvxSourceBindingFactory _sourceBindingFactory;
        private IMvxTargetBindingFactory _targetBindingFactory;
        private IMvxLanguageBindingParser _languageParser;
        private IMvxPropertyExpressionParser _propertyExpressionParser;
        private IMvxValueConverterLookup _valueConverterLookup;
        private IMvxBindingNameLookup _defaultBindingName;
        private IMvxBinder _binder;

        public IMvxLanguageBindingParser LanguageParser
        {
            get
            {
                _languageParser = _languageParser ?? Mvx.Resolve<IMvxLanguageBindingParser>();
                return _languageParser;
            }
        }

        public IMvxPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                _propertyExpressionParser = _propertyExpressionParser ?? Mvx.Resolve<IMvxPropertyExpressionParser>();
                return _propertyExpressionParser;
            }
        }

        public IMvxValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? Mvx.Resolve<IMvxValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        public IMvxBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                _defaultBindingName = _defaultBindingName ?? Mvx.Resolve<IMvxBindingNameLookup>();
                return _defaultBindingName;
            }
        }

        public IMvxBinder Binder
        {
            get
            {
                _binder = _binder ?? Mvx.Resolve<IMvxBinder>();
                return _binder;
            }
        }

        public IMvxSourceBindingFactory SourceBindingFactory
        {
            get
            {
                _sourceBindingFactory = _sourceBindingFactory ?? Mvx.Resolve<IMvxSourceBindingFactory>();
                return _sourceBindingFactory;
            }
        }

        public IMvxTargetBindingFactory TargetBindingFactory
        {
            get
            {
                _targetBindingFactory = _targetBindingFactory ?? Mvx.Resolve<IMvxTargetBindingFactory>();
                return _targetBindingFactory;
            }
        }
    }
}