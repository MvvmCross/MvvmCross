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
        : MvxApplicableTo<TTarget>
        where TTarget : class
    {
        private readonly TTarget _target;
        private readonly IMvxBindingContextOwner _bindingContextOwner;
        private MvxBindingDescription _bindingDescription = new MvxBindingDescription();

        protected MvxBindingDescription BindingDescription
        {
            get { return _bindingDescription; }
        }

        protected void Overwrite(MvxBindingDescription bindingDescription)
        {
            _bindingDescription = bindingDescription;
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

        public override void Apply()
        {
            EnsureTargetNameSet();
            _bindingContextOwner.AddBinding(_target, BindingDescription);
            base.Apply();
        }

        public override void ApplyTo(TTarget what)
        {
            EnsureTargetNameSet();
            _bindingContextOwner.AddBinding(what, BindingDescription);
            base.ApplyTo(what);
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