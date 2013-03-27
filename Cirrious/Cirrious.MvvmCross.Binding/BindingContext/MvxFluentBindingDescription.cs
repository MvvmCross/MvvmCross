// MvxFluentBindingDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.Binders;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxFluentBindingDescription<TTarget>
        : IMvxApplicable
          , IMvxApplicableTo<TTarget>
        where TTarget : class
    {
        private readonly TTarget _target;
        private readonly IMvxBindingContextOwner _bindingContextOwner;
        private readonly MvxBindingDescription _bindingDescription;

        public MvxFluentBindingDescription(IMvxBindingContextOwner bindingContextOwner, TTarget target = null)
        {
            _bindingContextOwner = bindingContextOwner;
            _target = target;
            _bindingDescription = new MvxBindingDescription
                {
                };
        }

        public MvxFluentBindingDescription<TTarget> For(string targetPropertyName)
        {
            _bindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var parser = MvxBindingSingletonCache.Instance.PropertyExpressionParser;
            var targetPropertyName = parser.Parse(targetPropertyPath).Print();
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
            _bindingDescription.Mode = mode;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To(string sourcePropertyPath)
        {
            _bindingDescription.SourcePropertyPath = sourcePropertyPath;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> To<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var parser = MvxBindingSingletonCache.Instance.PropertyExpressionParser;
            var sourcePropertyPath = parser.Parse(sourceProperty).Print();

            return To(sourcePropertyPath);
        }

        public MvxFluentBindingDescription<TTarget> WithConversion(string converterName,
                                                                   object converterParameter = null)
        {
            var converter = MvxBindingSingletonCache.Instance.ValueConverterLookup.Find(converterName);
            return WithConversion(converter, converterParameter);
        }

        public MvxFluentBindingDescription<TTarget> WithConversion(IMvxValueConverter converter,
                                                                   object converterParameter)
        {
            _bindingDescription.Converter = converter;
            _bindingDescription.ConverterParameter = converterParameter;
            return this;
        }

        public MvxFluentBindingDescription<TTarget> WithFallback(object fallback)
        {
            _bindingDescription.FallbackValue = fallback;
            return this;
        }

        public void Apply()
        {
            EnsureTargetNameSet();
            _bindingContextOwner.AddBinding(_target, _bindingDescription);
        }

        public void ApplyTo(TTarget what)
        {
            EnsureTargetNameSet();
            _bindingContextOwner.AddBinding(what, _bindingDescription);
        }

        private void EnsureTargetNameSet()
        {
            if (!string.IsNullOrEmpty(_bindingDescription.TargetName))
                return;

            _bindingDescription.TargetName =
                MvxBindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof (TTarget));
        }
    }
}