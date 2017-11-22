﻿// MvxFluentBindingDescription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Linq.Expressions;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Combiners;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding.BindingContext
{
    public class MvxFluentBindingDescription<TTarget, TSource>
        : MvxBaseFluentBindingDescription<TTarget>
        where TTarget : class
    {
        public MvxFluentBindingDescription(IMvxBindingContextOwner bindingContextOwner, TTarget target)
            : base(bindingContextOwner, target)
        {
        }

        public MvxFluentBindingDescription<TTarget, TSource> For(string targetPropertyName)
        {
            BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return For(targetPropertyName);
        }

        public MvxFluentBindingDescription<TTarget, TSource> TwoWay()
        {
            return Mode(MvxBindingMode.TwoWay);
        }

        public MvxFluentBindingDescription<TTarget, TSource> OneWay()
        {
            return Mode(MvxBindingMode.OneWay);
        }

        public MvxFluentBindingDescription<TTarget, TSource> OneWayToSource()
        {
            return Mode(MvxBindingMode.OneWayToSource);
        }

        public MvxFluentBindingDescription<TTarget, TSource> OneTime()
        {
            return Mode(MvxBindingMode.OneTime);
        }

        public MvxFluentBindingDescription<TTarget, TSource> Mode(MvxBindingMode mode)
        {
            BindingDescription.Mode = mode;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> To(string sourcePropertyPath)
        {
            SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> To(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> ByCombining(string combinerName, params Expression<Func<TSource, object>>[] properties)
            => ByCombining(combinerName, properties.Select(SourcePropertyPath).ToArray());

        public MvxFluentBindingDescription<TTarget, TSource> ByCombining(string combinerName, params string[] properties)
            => To($"{combinerName}({string.Join(", ", properties)})");

        public MvxFluentBindingDescription<TTarget, TSource> ByCombining(IMvxValueCombiner combiner, params Expression<Func<TSource, object>>[] properties)
        {
            SetCombiner(combiner, properties.Select(SourcePropertyPath).ToArray(), useParser: false);
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> ByCombining(IMvxValueCombiner combiner, params string[] properties)
        {
            SetCombiner(combiner, properties, useParser: true);
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> CommandParameter(object parameter)
        {
            return WithConversion(new MvxCommandParameterValueConverter(), parameter);
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithConversion(string converterName,
                                                                            object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return WithConversion(converter, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithConversion(IMvxValueConverter converter,
                                                                            object converterParameter = null)
        {
            SourceStepDescription.Converter = converter;
            SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithConversion<TValueConverter>(object converterParameter = null)
            where TValueConverter : IMvxValueConverter
        {
            var filler = Mvx.Resolve<IMvxValueConverterRegistryFiller>();
            var converterName = filler.FindName(typeof(TValueConverter));

            return WithConversion(converterName, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithFallback(object fallback)
        {
            SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        [Obsolete("Please use SourceDescribed or FullyDescribed instead")]
        public MvxFluentBindingDescription<TTarget, TSource> Described(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return Described(newBindingDescription);
        }

        [Obsolete("Please use SourceDescribed or FullyDescribed instead")]
        public MvxFluentBindingDescription<TTarget, TSource> Described(MvxBindingDescription description)
        {
            Overwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return SourceDescribed(newBindingDescription);
        }

        public MvxFluentBindingDescription<TTarget, TSource> SourceDescribed(MvxBindingDescription description)
        {
            SourceOverwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                MvxLog.Instance.Warn("More than one description found - only first will be used in {0}", bindingDescription);
            }

            return FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public MvxFluentBindingDescription<TTarget, TSource> FullyDescribed(MvxBindingDescription description)
        {
            FullOverwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithClearBindingKey(object clearBindingKey)
        {
            ClearBindingKey = clearBindingKey;
            return this;
        }
    }

    public class MvxFluentBindingDescription<TTarget>
        : MvxBaseFluentBindingDescription<TTarget>
        where TTarget : class
    {
        public MvxFluentBindingDescription(IMvxBindingContextOwner bindingContextOwner, TTarget target = null)
            : base(bindingContextOwner, target)
        {
        }

        public MvxFluentBindingDescription<TTarget> For(string targetPropertyName)
        {
            BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return For(targetPropertyName);
        }

        public MvxFluentBindingDescription<TTarget> TwoWay()
        {
            return Mode(MvxBindingMode.TwoWay);
        }

        public MvxFluentBindingDescription<TTarget> OneWay()
        {
            return Mode(MvxBindingMode.OneWay);
        }

        public MvxFluentBindingDescription<TTarget> OneWayToSource()
        {
            return Mode(MvxBindingMode.OneWayToSource);
        }

        public MvxFluentBindingDescription<TTarget> OneTime()
        {
            return Mode(MvxBindingMode.OneTime);
        }

        public MvxFluentBindingDescription<TTarget> Mode(MvxBindingMode mode)
        {
            BindingDescription.Mode = mode;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To(string sourcePropertyPath)
        {
            SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget> CommandParameter(object parameter)
        {
            return WithConversion(new MvxCommandParameterValueConverter(), parameter);
        }

        public MvxFluentBindingDescription<TTarget> WithConversion(string converterName,
                                                                   object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return WithConversion(converter, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget> WithConversion(IMvxValueConverter converter,
                                                                   object converterParameter)
        {
            SourceStepDescription.Converter = converter;
            SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> WithConversion<TValueConverter>(object converterParameter = null)
            where TValueConverter : IMvxValueConverter
        {
            var filler = Mvx.Resolve<IMvxValueConverterRegistryFiller>();
            var converterName = filler.FindName(typeof(TValueConverter));

            return WithConversion(converterName, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget> WithFallback(object fallback)
        {
            SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        [Obsolete("Please use SourceDescribed or FullyDescribed instead")]
        public MvxFluentBindingDescription<TTarget> Described(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return Described(newBindingDescription);
        }

        [Obsolete("Please use SourceDescribed or FullyDescribed instead")]
        public MvxFluentBindingDescription<TTarget> Described(MvxBindingDescription description)
        {
            Overwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return SourceDescribed(newBindingDescription);
        }

        public MvxFluentBindingDescription<TTarget> SourceDescribed(MvxBindingDescription description)
        {
            SourceOverwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                MvxLog.Instance.Warn("More than one description found - only first will be used in {0}", bindingDescription);
            }

            return FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public MvxFluentBindingDescription<TTarget> FullyDescribed(MvxBindingDescription description)
        {
            FullOverwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget> WithClearBindingKey(object clearBindingKey)
        {
            ClearBindingKey = clearBindingKey;
            return this;
        }
    }
}