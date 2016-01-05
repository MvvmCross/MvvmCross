// MvxFluentBindingDescription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using MvvmCross.Binding.Bindings;
    using MvvmCross.Binding.ValueConverters;
    using MvvmCross.Platform.Converters;

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
            this.BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return this.For(targetPropertyName);
        }

        public MvxFluentBindingDescription<TTarget, TSource> TwoWay()
        {
            return this.Mode(MvxBindingMode.TwoWay);
        }

        public MvxFluentBindingDescription<TTarget, TSource> OneWay()
        {
            return this.Mode(MvxBindingMode.OneWay);
        }

        public MvxFluentBindingDescription<TTarget, TSource> OneWayToSource()
        {
            return this.Mode(MvxBindingMode.OneWayToSource);
        }

        public MvxFluentBindingDescription<TTarget, TSource> OneTime()
        {
            return this.Mode(MvxBindingMode.OneTime);
        }

        public MvxFluentBindingDescription<TTarget, TSource> Mode(MvxBindingMode mode)
        {
            this.BindingDescription.Mode = mode;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> To(string sourcePropertyPath)
        {
            this.SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> To(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            this.SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> CommandParameter(object parameter)
        {
            return this.WithConversion(new MvxCommandParameterValueConverter(), parameter);
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithConversion(string converterName,
                                                                            object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return this.WithConversion(converter, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithConversion(IMvxValueConverter converter,
                                                                            object converterParameter = null)
        {
            this.SourceStepDescription.Converter = converter;
            this.SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithFallback(object fallback)
        {
            this.SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        [Obsolete("Please use SourceDescribed or FullyDescribed instead")]
        public MvxFluentBindingDescription<TTarget, TSource> Described(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return this.Described(newBindingDescription);
        }

        [Obsolete("Please use SourceDescribed or FullyDescribed instead")]
        public MvxFluentBindingDescription<TTarget, TSource> Described(MvxBindingDescription description)
        {
            this.Overwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return this.SourceDescribed(newBindingDescription);
        }

        public MvxFluentBindingDescription<TTarget, TSource> SourceDescribed(MvxBindingDescription description)
        {
            this.SourceOverwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                MvxBindingTrace.Warning("More than one description found - only first will be used in {0}", bindingDescription);
            }

            return this.FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public MvxFluentBindingDescription<TTarget, TSource> FullyDescribed(MvxBindingDescription description)
        {
            this.FullOverwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithClearBindingKey(object clearBindingKey)
        {
            this.ClearBindingKey = clearBindingKey;
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
            this.BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return this.For(targetPropertyName);
        }

        public MvxFluentBindingDescription<TTarget> TwoWay()
        {
            return this.Mode(MvxBindingMode.TwoWay);
        }

        public MvxFluentBindingDescription<TTarget> OneWay()
        {
            return this.Mode(MvxBindingMode.OneWay);
        }

        public MvxFluentBindingDescription<TTarget> OneWayToSource()
        {
            return this.Mode(MvxBindingMode.OneWayToSource);
        }

        public MvxFluentBindingDescription<TTarget> OneTime()
        {
            return this.Mode(MvxBindingMode.OneTime);
        }

        public MvxFluentBindingDescription<TTarget> Mode(MvxBindingMode mode)
        {
            this.BindingDescription.Mode = mode;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To(string sourcePropertyPath)
        {
            this.SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            this.SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public MvxFluentBindingDescription<TTarget> CommandParameter(object parameter)
        {
            return this.WithConversion(new MvxCommandParameterValueConverter(), parameter);
        }

        public MvxFluentBindingDescription<TTarget> WithConversion(string converterName,
                                                                   object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return this.WithConversion(converter, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget> WithConversion(IMvxValueConverter converter,
                                                                   object converterParameter)
        {
            this.SourceStepDescription.Converter = converter;
            this.SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> WithFallback(object fallback)
        {
            this.SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        [Obsolete("Please use SourceDescribed or FullyDescribed instead")]
        public MvxFluentBindingDescription<TTarget> Described(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return this.Described(newBindingDescription);
        }

        [Obsolete("Please use SourceDescribed or FullyDescribed instead")]
        public MvxFluentBindingDescription<TTarget> Described(MvxBindingDescription description)
        {
            this.Overwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return this.SourceDescribed(newBindingDescription);
        }

        public MvxFluentBindingDescription<TTarget> SourceDescribed(MvxBindingDescription description)
        {
            this.SourceOverwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                MvxBindingTrace.Warning("More than one description found - only first will be used in {0}", bindingDescription);
            }

            return this.FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public MvxFluentBindingDescription<TTarget> FullyDescribed(MvxBindingDescription description)
        {
            this.FullOverwrite(description ?? new MvxBindingDescription());
            return this;
        }

        public MvxFluentBindingDescription<TTarget> WithClearBindingKey(object clearBindingKey)
        {
            this.ClearBindingKey = clearBindingKey;
            return this;
        }
    }
}