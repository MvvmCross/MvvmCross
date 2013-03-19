// MvxBindingContextOwnerExtensions.CachedStatics.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces.ExpressionParse;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static void ClearCaches()
        {
            _languageParser = null;
            _propertyExpressionParser = null;
            _valueConverterLookup = null;
            _defaultBindingName = null;
            _binder = null;
        }

        private static IMvxLanguageBindingParser _languageParser;
        private static IMvxPropertyExpressionParser _propertyExpressionParser;
        private static IMvxValueConverterLookup _valueConverterLookup;
        private static IMvxBindingNameLookup _defaultBindingName;
        private static IMvxBinder _binder;

        private static IMvxLanguageBindingParser LanguageParser
        {
            get
            {
                _languageParser = _languageParser ?? Mvx.Resolve<IMvxLanguageBindingParser>();
                return _languageParser;
            }
        }

        private static IMvxPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                _propertyExpressionParser = _propertyExpressionParser ?? Mvx.Resolve<IMvxPropertyExpressionParser>();
                return _propertyExpressionParser;
            }
        }

        private static IMvxValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? Mvx.Resolve<IMvxValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        private static IMvxBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                _defaultBindingName = _defaultBindingName ?? Mvx.Resolve<IMvxBindingNameLookup>();
                return _defaultBindingName;
            }
        }

        private static IMvxBinder Binder
        {
            get
            {
                _binder = _binder ?? Mvx.Resolve<IMvxBinder>();
                return _binder;
            }
        }
    }
}