// MvxBindingContextOwnerExtensions.Fluent.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static MvxFluentBindingDescription<TTarget> CreateBinding<TTarget>(this TTarget target)
            where TTarget : class, IMvxBindingContextOwner
        {
            return new MvxFluentSelfBindingDescription<TTarget>(target);
        }

        public static MvxFluentBindingDescription<TTarget> CreateBinding<TTarget>(
            this IMvxBindingContextOwner contextOwner, TTarget target)
            where TTarget : class
        {
            return new MvxFluentBindingDescription<TTarget>(contextOwner, target);
        }
    }

#warning Remove these
    /*
    public static partial class MvxBindingContextOwnerExtensions
    {
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
            bindingOwner.Bind(target, targetProperty, sourcePropertyPath, converter, converterParameter, fallbackValue,
                              mode);
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

            bindingOwner.Bind(target, converter, converterParameter, fallbackValue, mode, parsedTargetPath, parsedSource);
        }

        public static void Bind<TTarget, TSource>(this TTarget bindingOwner,
                                                  Expression<Func<TSource, object>> sourcePropertyPath,
                                                  string converterName,
                                                  object converterParameter = null,
                                                  object fallbackValue = null,
                                                  MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class, IMvxBindingContextOwner
        {
            var converter = ValueConverterLookup.Find(converterName);
            bindingOwner.Bind(bindingOwner, string.Empty, sourcePropertyPath, converter, converterParameter,
                              fallbackValue, mode);
        }

        public static void Bind<TTarget, TSource>(this TTarget bindingOwner,
                                                  Expression<Func<TSource, object>> sourcePropertyPath,
                                                  IMvxValueConverter converter = null,
                                                  object converterParameter = null,
                                                  object fallbackValue = null,
                                                  MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class, IMvxBindingContextOwner
        {
            bindingOwner.Bind(bindingOwner, string.Empty, sourcePropertyPath, converter, converterParameter,
                              fallbackValue, mode);
        }

        public static void Bind<TTarget, TSource>(this IMvxBindingContextOwner bindingOwner,
                                                  TTarget target,
                                                  Expression<Func<TSource, object>> sourcePropertyPath,
                                                  string converterName,
                                                  object converterParameter = null,
                                                  object fallbackValue = null,
                                                  MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class
        {
            var converter = ValueConverterLookup.Find(converterName);
            bindingOwner.Bind(target, string.Empty, sourcePropertyPath, converter, converterParameter, fallbackValue,
                              mode);
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
            bindingOwner.Bind(target, string.Empty, sourcePropertyPath, converter, converterParameter, fallbackValue,
                              mode);
        }

        public static void Bind<TTarget, TSource>(this TTarget bindingOwner,
                                                  string eventOrPropertyName,
                                                  Expression<Func<TSource, object>> sourcePropertyPath,
                                                  string converterName,
                                                  object converterParameter = null,
                                                  object fallbackValue = null,
                                                  MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class, IMvxBindingContextOwner
        {
            var converter = ValueConverterLookup.Find(converterName);
            bindingOwner.Bind(bindingOwner, eventOrPropertyName, sourcePropertyPath, converter, converterParameter,
                              fallbackValue, mode);
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
            bindingOwner.Bind(bindingOwner, eventOrPropertyName, sourcePropertyPath, converter, converterParameter,
                              fallbackValue, mode);
        }


        public static void Bind<TTarget, TSource>(this IMvxBindingContextOwner bindingOwner,
                                                  TTarget target,
                                                  string eventOrPropertyName,
                                                  Expression<Func<TSource, object>> sourcePropertyPath,
                                                  string converterName,
                                                  object converterParameter = null,
                                                  object fallbackValue = null,
                                                  MvxBindingMode mode = MvxBindingMode.Default)
            where TTarget : class
        {
            var converter = ValueConverterLookup.Find(converterName);
            bindingOwner.Bind(target, eventOrPropertyName, sourcePropertyPath, converter, converterParameter,
                              fallbackValue, mode);
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

            if (string.IsNullOrEmpty(eventOrPropertyName))
            {
                eventOrPropertyName = DefaultBindingNameLookup.DefaultFor(typeof (TTarget));
            }

            bindingOwner.Bind(target, converter, converterParameter, fallbackValue, mode, eventOrPropertyName,
                              parsedSource.Print());
        }

        private static void Bind(this IMvxBindingContextOwner bindingOwner, object target, IMvxValueConverter converter,
                                 object converterParameter, object fallbackValue, MvxBindingMode mode,
                                 IMvxParsedExpression parsedTargetPath, IMvxParsedExpression parsedSourcePath)
        {
            bindingOwner.Bind(target, converter, converterParameter, fallbackValue, mode, parsedTargetPath.Print(),
                              parsedSourcePath.Print());
        }

        private static void Bind(this IMvxBindingContextOwner bindingOwner, object target, IMvxValueConverter converter,
                                 object converterParameter, object fallbackValue, MvxBindingMode mode,
                                 string parsedTargetPath, string parsedSourcePath)
        {
            var description = new MvxBindingDescription
                {
                    TargetName = parsedTargetPath,
                    SourcePropertyPath = parsedSourcePath,
                    Converter = converter,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    Mode = mode
                };
            bindingOwner.AddBinding(target, description);
        }
    }
     */
}