// MvxBindingContextOwnerExpressionExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.Converters;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Binding.ExpressionParse;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces.ExpressionParse;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static class MvxBindingContextOwnerExpressionExtensions
    {
        private static IMvxPropertyExpressionParser _propertyExpressionParser;

        private static IMvxPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                if (_propertyExpressionParser == null)
                    _propertyExpressionParser = new MvxPropertyExpressionParser();
                return _propertyExpressionParser;
            }
        }

        private static IMvxValueConverterLookup _valueConverterLookup;

        private static IMvxValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? Mvx.Resolve<IMvxValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        public static void Bind<TTarget, TSource>(this TTarget target,
                                         Expression<Func<TTarget, object>> targetProperty,
                                         Expression<Func<TSource, object>> sourcePropertyPath,
                                         string converterName,
                                         object converterParameter = null,
                                         object fallbackValue = null,
                                         MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class, IMvxBindingContextOwner
        {
            var converter = ValueConverterLookup.Find(converterName);
            target.Bind(target, targetProperty, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
        }

        public static void Bind<TTarget, TSource>(this IMvxBindingContextOwner bindingOwner,
                                                  TTarget target,
                                                  Expression<Func<TTarget, object>> targetProperty,
                                                  Expression<Func<TSource, object>> sourcePropertyPath,
                                                  string converterName,
                                                  object converterParameter = null,
                                                  object fallbackValue = null,
                                                  MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class
        {
            var converter = ValueConverterLookup.Find(converterName);
            bindingOwner.Bind(target, targetProperty, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
        }

        public static void Bind<TTarget, TSource>(this TTarget target,
                                         Expression<Func<TTarget, object>> targetProperty,
                                         Expression<Func<TSource, object>> sourcePropertyPath,
                                         IMvxValueConverter converter = null,
                                         object converterParameter = null,
                                         object fallbackValue = null,
                                         MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class, IMvxBindingContextOwner
        {
            target.Bind(target, targetProperty, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
        }

        public static void Bind<TTarget, TSource>(this IMvxBindingContextOwner bindingOwner,
                                         TTarget target,
                                         Expression<Func<TTarget, object>> targetProperty,
                                         Expression<Func<TSource, object>> sourcePropertyPath,
                                         IMvxValueConverter converter = null,
                                         object converterParameter = null,
                                         object fallbackValue = null,
                                         MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class
        {
            var parser = PropertyExpressionParser;

            var parsedTargetPath = parser.Parse(targetProperty);
            var parsedSource = parser.Parse(sourcePropertyPath);

            var description = new MvxBindingDescription
                {
                    TargetName = parsedTargetPath.Print(),
                    SourcePropertyPath = parsedSource.Print(),
                    Converter = converter,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    Mode = mode
                };

            bindingOwner.AddBinding(target, description);
        }
    }
}