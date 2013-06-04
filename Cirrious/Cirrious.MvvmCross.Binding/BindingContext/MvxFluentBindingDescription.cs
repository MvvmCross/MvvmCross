// MvxFluentBindingDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.ValueConverters;

namespace Cirrious.MvvmCross.Binding.BindingContext
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
            BindingDescription.SourcePropertyPath = sourcePropertyPath;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> To(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            return To(sourcePropertyPath);
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
                                                                   object converterParameter)
        {
            BindingDescription.Converter = converter;
            BindingDescription.ConverterParameter = converterParameter;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> WithFallback(object fallback)
        {
            BindingDescription.FallbackValue = fallback;
            return this;
        }

        public MvxFluentBindingDescription<TTarget, TSource> Described(string bindingDescription)
        {
            var newBindingDescription = MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return Described(newBindingDescription);
        }

        public MvxFluentBindingDescription<TTarget, TSource> Described(MvxBindingDescription description)
        {
            Overwrite(description ?? new MvxBindingDescription());
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
            BindingDescription.SourcePropertyPath = sourcePropertyPath;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            return To(sourcePropertyPath);
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
            BindingDescription.Converter = converter;
            BindingDescription.ConverterParameter = converterParameter;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> WithFallback(object fallback)
        {
            BindingDescription.FallbackValue = fallback;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> Described(string bindingDescription)
        {
            var newBindingDescription = MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return Described(newBindingDescription);
        }

        public MvxFluentBindingDescription<TTarget> Described(MvxBindingDescription description)
        {
            Overwrite(description ?? new MvxBindingDescription());
            return this;
        }
    }
}