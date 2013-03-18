// MvxBindingContextOwnerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using Cirrious.CrossCore.Interfaces.Converters;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;

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

        private static IMvxValueConverterProvider _valueConverterProvider;
        private static IMvxValueConverterProvider ValueConverterProvider
        {
            get
            {
                _valueConverterProvider = _valueConverterProvider ?? Mvx.Resolve<IMvxValueConverterProvider>();
                return _valueConverterProvider;
            }
        }

        public static void AddBinding<T>(this IMvxBindingContextOwner view,
                                         Expression<Func<T>> property,
                                         Expression<T> contextPropertyPath,
                                         string converterName = "",
                                         object converterParameter = null,
                                         object fallbackValue = null,
                                         MvxBindingMode mode = MvxBindingMode.Default)
        {
            view.AddBinding(view, property, contextPropertyPath, converterName, converterParameter, fallbackValue, mode);
        }

        public static void AddBinding<T>(this IMvxBindingContextOwner view,
                                         object target,
                                         Expression<Func<T>> property,
                                         Expression<T> contextPropertyPath,
                                         string converterName = "",
                                         object converterParameter = null,
                                         object fallbackValue = null,
                                         MvxBindingMode mode = MvxBindingMode.Default)
        {
            var valueConverter = ValueConverterProvider.Find(converterName);
            view.AddBinding(target, property, contextPropertyPath, valueConverter, converterParameter, fallbackValue, mode);
        }

        public static void AddBinding<T>(this IMvxBindingContextOwner view,
                                         Expression<Func<T>> property,
                                         Expression<T> contextPropertyPath,
                                         IMvxValueConverter valueConverter = null,
                                         object converterParameter = null,
                                         object fallbackValue = null,
                                         MvxBindingMode mode = MvxBindingMode.Default)
        {
            view.AddBinding(view, property, contextPropertyPath, valueConverter, converterParameter, fallbackValue, mode);
        }

        public static void AddBinding<T>(this IMvxBindingContextOwner view, 
                                         object target, 
                                         Expression<Func<T>> property, 
                                         Expression<T> contextPropertyPath,
                                         IMvxValueConverter converter = null,
                                         object converterParameter = null,
                                         object fallbackValue = null,
                                         MvxBindingMode mode = MvxBindingMode.Default)
        {
            var parser = PropertyExpressionParser;

            var parsedTargetPath = parser.Parse(contextPropertyPath);
            var parsedSource = parser.Parse(contextPropertyPath);

            var description = new MvxBindingDescription()
                {
                    TargetName = parsedTargetPath.Print(),
                    SourcePropertyPath = parsedSource.Print(),
                    Converter = converter,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    Mode = mode
                };

            view.AddBinding(target, description);
        }
    }
}