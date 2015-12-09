// MvxBindExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Bindings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static class MvxBindExtensions
    {
        public static MvxInlineBindingTarget<TViewModel> CreateInlineBindingTarget<TViewModel>(
            this IMvxBindingContextOwner bindingContextOwner)
        {
            return new MvxInlineBindingTarget<TViewModel>(bindingContextOwner);
        }

        public static T Bind<T, TViewModel>(this T element, MvxInlineBindingTarget<TViewModel> target,
                                            string descriptionText)
        {
            target.BindingContextOwner.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            string converterName = null,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            return element.Bind(target, null, sourcePropertyPath, converterName, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            IMvxValueConverter converter,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            return element.Bind(target, null, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            Expression<Func<T, object>> targetPropertyPath,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            string converterName = null,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            var converter = MvxBindingSingletonCache.Instance.ValueConverterLookup.Find(converterName);
            return element.Bind(target, targetPropertyPath, sourcePropertyPath, converter, converterParameter,
                                fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            Expression<Func<T, object>> targetPropertyPath,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            IMvxValueConverter converter,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            var parser = MvxBindingSingletonCache.Instance.PropertyExpressionParser;
            var sourcePath = parser.Parse(sourcePropertyPath).Print();
            var targetPath = targetPropertyPath == null ? null : parser.Parse(targetPropertyPath).Print();
            return element.Bind(target, targetPath, sourcePath, converter, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            MvxInlineBindingTarget<TViewModel> target,
                                            string targetPath,
                                            string sourcePath,
                                            IMvxValueConverter converter = null,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            MvxBindingMode mode = MvxBindingMode.Default)
        {
            if (string.IsNullOrEmpty(targetPath))
                targetPath = MvxBindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof(T));

            var bindingDescription = new MvxBindingDescription(
                targetPath,
                sourcePath,
                converter,
                converterParameter,
                fallbackValue,
                mode);

            target.BindingContextOwner.AddBinding(element, bindingDescription);

            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingContextOwner bindingContextOwner, string descriptionText)
        {
            bindingContextOwner.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IMvxBindingContextOwner bindingContextOwner,
                                IEnumerable<MvxBindingDescription> descriptions)
        {
            bindingContextOwner.AddBindings(element, descriptions);
            return element;
        }
    }
}