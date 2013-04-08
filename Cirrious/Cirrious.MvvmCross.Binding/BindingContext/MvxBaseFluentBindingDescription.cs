// MvxBaseFluentBindingDescription.cs
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
    public class MvxBaseFluentBindingDescription<TTarget>
        : IMvxApplicable
          , IMvxApplicableTo<TTarget>
        where TTarget : class
    {
        private readonly TTarget _target;
        private readonly MvxBindingDescription _bindingDescription = new MvxBindingDescription();
        private readonly IMvxBindingContextOwner _bindingContextOwner;

        protected MvxBindingDescription BindingDescription
        {
            get { return _bindingDescription; }
        }

        public MvxBaseFluentBindingDescription(IMvxBindingContextOwner bindingContextOwner, TTarget target)
        {
            _bindingContextOwner = bindingContextOwner;
            _target = target;
        }

        protected static string TargetPropertyName(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var parser = MvxBindingSingletonCache.Instance.PropertyExpressionParser;
            var targetPropertyName = parser.Parse(targetPropertyPath).Print();
            return targetPropertyName;
        }

        protected static string SourcePropertyPath<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var parser = MvxBindingSingletonCache.Instance.PropertyExpressionParser;
            var sourcePropertyPath = parser.Parse(sourceProperty).Print();
            return sourcePropertyPath;
        }

        protected static IMvxValueConverter ValueConverterFromName(string converterName)
        {
            var converter = MvxBindingSingletonCache.Instance.ValueConverterLookup.Find(converterName);
            return converter;
        }

        public void Apply()
        {
            EnsureTargetNameSet();
            _bindingContextOwner.AddBinding(_target, BindingDescription);
        }

        public void ApplyTo(TTarget what)
        {
            EnsureTargetNameSet();
            _bindingContextOwner.AddBinding(what, BindingDescription);
        }

        protected void EnsureTargetNameSet()
        {
            if (!string.IsNullOrEmpty(BindingDescription.TargetName))
                return;

            BindingDescription.TargetName =
                MvxBindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
        }
    }
}