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
                    _propertyExpressionParser = Mvx.Resolve<IMvxPropertyExpressionParser>();
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

		private static IMvxBindingNameLookup _defaultBindingName;
		
		private static IMvxBindingNameLookup DefaultBindingNameLookup
		{
			get
			{
				if (_defaultBindingName == null)
					_defaultBindingName = Mvx.Resolve<IMvxBindingNameLookup>();
				return _defaultBindingName;
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

			bindingOwner.Bind (target, converter, converterParameter, fallbackValue, mode, parsedTargetPath, parsedSource);

        }

		public static void Bind<TTarget, TSource>(this TTarget bindingOwner,
		                                          Expression<Func<TSource, object>> sourcePropertyPath,
		                                          IMvxValueConverter converter = null,
		                                          object converterParameter = null,
		                                          object fallbackValue = null,
		                                          MvxBindingMode mode = MvxBindingMode.Default)
			where TTarget : class, IMvxBindingContextOwner
		{
			bindingOwner.Bind (bindingOwner, string.Empty, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
		}

		public static void Bind<TTarget, TSource>(this IMvxBindingContextOwner bindingOwner,
		                                          TTarget target,
		                                          Expression<Func<TSource, object>> sourcePropertyPath,
		                                          IMvxValueConverter converter = null,
		                                          object converterParameter = null,
		                                          object fallbackValue = null,
		                                          MvxBindingMode mode = MvxBindingMode.Default)
			where TTarget : class
		{
			bindingOwner.Bind (target, string.Empty, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
		}

		public static void Bind<TTarget, TSource>(this TTarget bindingOwner,
		                                          string eventOrPropertyName,
		                                          Expression<Func<TSource, object>> sourcePropertyPath,
		                                          IMvxValueConverter converter = null,
		                                          object converterParameter = null,
		                                          object fallbackValue = null,
		                                          MvxBindingMode mode = MvxBindingMode.Default)
			where TTarget : class, IMvxBindingContextOwner
		{
			bindingOwner.Bind (bindingOwner, string.Empty, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
		}

		public static void Bind<TTarget, TSource>(this IMvxBindingContextOwner bindingOwner,
		                                          TTarget target,
		                                          string eventOrPropertyName,
		                                          Expression<Func<TSource, object>> sourcePropertyPath,
		                                          IMvxValueConverter converter = null,
		                                          object converterParameter = null,
		                                          object fallbackValue = null,
		                                          MvxBindingMode mode = MvxBindingMode.Default)
			where TTarget : class
		{
			var parser = PropertyExpressionParser;			
			var parsedSource = parser.Parse(sourcePropertyPath);

			if (string.IsNullOrEmpty (eventOrPropertyName)) {
				eventOrPropertyName = DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
			}

			bindingOwner.Bind (target, converter, converterParameter, fallbackValue, mode, eventOrPropertyName, parsedSource.Print());			
		}

		private static void Bind(this IMvxBindingContextOwner bindingOwner, object target, IMvxValueConverter converter, object converterParameter, object fallbackValue, MvxBindingMode mode, IMvxParsedExpression parsedTargetPath, IMvxParsedExpression parsedSourcePath)
		{
			bindingOwner.Bind (target, converter, converterParameter, fallbackValue, mode, parsedTargetPath.Print (), parsedSourcePath.Print ());
		}

		private static void Bind(this IMvxBindingContextOwner bindingOwner, object target, IMvxValueConverter converter, object converterParameter, object fallbackValue, MvxBindingMode mode, string parsedTargetPath, string parsedSourcePath)
		{
			var description = new MvxBindingDescription {
				TargetName = parsedTargetPath,
				SourcePropertyPath = parsedSourcePath,
				Converter = converter,
				ConverterParameter = converterParameter,
				FallbackValue = fallbackValue,
				Mode = mode
			};
			bindingOwner.AddBinding (target, description);
		}
	}
}