// MvxBindingContextOwnerExtensions.Language.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Binders;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static void BindLanguage(this IMvxBindingContextOwner owner
                                        , string targetPropertyName
                                        , string sourceKey
                                        , string sourcePropertyName = null
                                        , string fallbackValue = null
                                        , string converterName = null)
        {
            owner.BindLanguage(owner, targetPropertyName, sourceKey, sourcePropertyName, fallbackValue, converterName);
        }

        public static void BindLanguage(this IMvxBindingContextOwner owner
                                        , object target
                                        , string targetPropertyName
                                        , string sourceKey
                                        , string sourcePropertyName = null
                                        , string fallbackValue = null
                                        , string converterName = null)
        {
            converterName = converterName ?? LanguageParser.DefaultConverterName;
            sourcePropertyName = sourcePropertyName ?? LanguageParser.DefaultTextSourceName;

            var converter = ValueConverterLookup.Find(converterName);

            var bindingDescription = new MvxBindingDescription
                {
                    TargetName = targetPropertyName,
                    SourcePropertyPath = sourcePropertyName,
                    Converter = converter,
                    ConverterParameter = sourceKey,
                    FallbackValue = fallbackValue,
                    Mode = MvxBindingMode.OneWay
                };
            owner.AddBinding(target, bindingDescription);
        }

        public static void AddLangBindings(this IMvxBindingContextOwner view, object target, string bindingText)
        {
            var bindings = Binder.LanguageBind(view.BindingContext.DataContext, target, bindingText);
            view.AddBindings(bindings);
        }

        public static void AddLangBindings(this IMvxBindingContextOwner view, IDictionary<object, string> lookup)
        {
            foreach (var kvp in lookup)
                view.AddLangBindings(kvp.Key, kvp.Value);
        }
    }
}