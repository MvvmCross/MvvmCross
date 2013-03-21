// MvxBindingStatics.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.IoC;
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
    public static class MvxBindingStatics
    {
        private static IMvxSourceBindingFactory _sourceBindingFactory;
        private static IMvxTargetBindingFactory _targetBindingFactory;
        private static IMvxLanguageBindingParser _languageParser;
        private static IMvxPropertyExpressionParser _propertyExpressionParser;
        private static IMvxValueConverterLookup _valueConverterLookup;
        private static IMvxBindingNameLookup _defaultBindingName;
        private static IMvxBinder _binder;

        public static void ClearCaches()
        {
            _sourceBindingFactory = null;
            _targetBindingFactory = null;
            _languageParser = null;
            _propertyExpressionParser = null;
            _valueConverterLookup = null;
            _defaultBindingName = null;
            _binder = null;
        }

        public static IMvxLanguageBindingParser LanguageParser
        {
            get
            {
                _languageParser = _languageParser ?? Mvx.Resolve<IMvxLanguageBindingParser>();
                return _languageParser;
            }
        }

        public static IMvxPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                _propertyExpressionParser = _propertyExpressionParser ?? Mvx.Resolve<IMvxPropertyExpressionParser>();
                return _propertyExpressionParser;
            }
        }

        public static IMvxValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? Mvx.Resolve<IMvxValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        public static IMvxBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                _defaultBindingName = _defaultBindingName ?? Mvx.Resolve<IMvxBindingNameLookup>();
                return _defaultBindingName;
            }
        }

        public static IMvxBinder Binder
        {
            get
            {
                _binder = _binder ?? Mvx.Resolve<IMvxBinder>();
                return _binder;
            }
        }

        public static IMvxSourceBindingFactory SourceBindingFactory
        {
            get
            {
                _sourceBindingFactory = _sourceBindingFactory ?? Mvx.Resolve<IMvxSourceBindingFactory>();
                return _sourceBindingFactory;
            }
        }

        public static IMvxTargetBindingFactory TargetBindingFactory
        {
            get
            {
                _targetBindingFactory = _targetBindingFactory ?? Mvx.Resolve<IMvxTargetBindingFactory>();
                return _targetBindingFactory;
            }
        }
    }
}