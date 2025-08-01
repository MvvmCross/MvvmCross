// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Localization;

namespace MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        // note that we don't add more default parameters here
        // - otherwise this overrides the other existing methods
        public static void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(this IMvxBindingContextOwner owner
                                                 , TTarget target
                                                 , string sourceKey)
        {
            var targetPath = MvxBindingSingletonCache.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
            owner.BindLanguage(target, targetPath, sourceKey);
        }

        public static void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(this IMvxBindingContextOwner owner
                                                 , TTarget target
                                                 , string sourceKey
                                                 , MvxBindingMode bindingMode)
        {
            var targetPath = MvxBindingSingletonCache.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
            owner.BindLanguage(target, targetPath, sourceKey, bindingMode: bindingMode);
        }

        public static void BindLanguage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TViewModel>(this IMvxBindingContextOwner owner
                                                             , TTarget target
                                                             , string sourceKey
                                                             , Expression<Func<TViewModel, IMvxTextProvider>> textProvider
                                                             , MvxBindingMode bindingMode = MvxBindingMode.OneTime)
        {
            var parser = PropertyExpressionParser;
            var targetPath = MvxBindingSingletonCache.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
            var sourcePath = parser.Parse(textProvider).Print();
            owner.BindLanguage(target, targetPath, sourceKey, sourcePath, bindingMode: bindingMode);
        }

        public static void BindLanguage<TTarget>(this IMvxBindingContextOwner owner
                                                 , TTarget target
                                                 , Expression<Func<TTarget, object>> targetPropertyExpression
                                                 , string sourceKey
                                                 , string sourcePropertyName = null
                                                 , string fallbackValue = null
                                                 , string converterName = null
                                                 , MvxBindingMode bindingMode = MvxBindingMode.OneTime)
        {
            var parser = PropertyExpressionParser;
            var parsedTargetPath = parser.Parse(targetPropertyExpression);
            var parsedTargetPathText = parsedTargetPath.Print();
            owner.BindLanguage(target, parsedTargetPathText, sourceKey, sourcePropertyName, fallbackValue, converterName, bindingMode);
        }

        public static void BindLanguage<TTarget, TViewModel>(this IMvxBindingContextOwner owner
                                                             , TTarget target
                                                             ,
                                                             Expression<Func<TTarget, object>> targetPropertyExpression
                                                             , string sourceKey
                                                             ,
                                                             Expression<Func<TViewModel, IMvxLanguageBinder>> sourcePropertyExpression
                                                             , string fallbackValue = null
                                                             , string converterName = null
                                                             , MvxBindingMode bindingMode = MvxBindingMode.OneTime)
        {
            var parser = PropertyExpressionParser;
            var parsedTargetPath = parser.Parse(targetPropertyExpression);
            var parsedTargetPathText = parsedTargetPath.Print();
            var parsedSourcePath = parser.Parse(sourcePropertyExpression);
            var sourcePropertyName = parsedSourcePath.Print();
            owner.BindLanguage(target, parsedTargetPathText, sourceKey, sourcePropertyName, fallbackValue, converterName, bindingMode);
        }

        public static void BindLanguage(this IMvxBindingContextOwner owner
                                        , string targetPropertyName
                                        , string sourceKey
                                        , string sourcePropertyName = null
                                        , string fallbackValue = null
                                        , string converterName = null
                                        , MvxBindingMode bindingMode = MvxBindingMode.OneTime)
        {
            owner.BindLanguage(owner, targetPropertyName, sourceKey, sourcePropertyName, fallbackValue, converterName, bindingMode);
        }

        public static void BindLanguage(this IMvxBindingContextOwner owner
                                        , object target
                                        , string targetPropertyName
                                        , string sourceKey
                                        , string sourcePropertyName = null
                                        , string fallbackValue = null
                                        , string converterName = null
                                        , MvxBindingMode bindingMode = MvxBindingMode.OneTime)
        {
            converterName ??= LanguageParser.DefaultConverterName;
            sourcePropertyName ??= LanguageParser.DefaultTextSourceName;

            var converter = ValueConverterLookup.Find(converterName);

            var bindingDescription = new MvxBindingDescription
            {
                TargetName = targetPropertyName,
                Source = new MvxPathSourceStepDescription
                {
                    SourcePropertyPath = sourcePropertyName,
                    Converter = converter,
                    ConverterParameter = sourceKey,
                    FallbackValue = fallbackValue,
                },
                Mode = bindingMode
            };
            owner.AddBinding(target, bindingDescription);
        }

        public static void AddLangBindings(this IMvxBindingContextOwner view, object target, string bindingText)
        {
            var bindings = Binder.LanguageBind(view.BindingContext.DataContext, target, bindingText);
            view.AddBindings(target, bindings);
        }

        public static void AddLangBindings(this IMvxBindingContextOwner view, IDictionary<object, string> lookup)
        {
            foreach (var kvp in lookup)
                view.AddLangBindings(kvp.Key, kvp.Value);
        }
    }
}
