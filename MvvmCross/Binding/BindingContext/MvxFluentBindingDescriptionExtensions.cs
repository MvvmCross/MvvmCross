// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Binding.Binders;
using MvvmCross.Converters;
using MvvmCross.Localization;

namespace MvvmCross.Binding.BindingContext
{
    public static class MvxFluentBindingDescriptionExtensions
    {
        public static MvxFluentBindingDescription<TTarget, TSource> ToLocalizationId<TTarget, TSource>(
            this MvxFluentBindingDescription<TTarget, TSource> bindingDescription,
            string localizationId)
            where TSource : IMvxLocalizedTextSourceOwner
            where TTarget : class
        {
            var valueConverter = Mvx.IoCProvider.Resolve<IMvxValueConverterLookup>().Find("Language");
            return bindingDescription.To(vm => vm.LocalizedTextSource)
                .OneTime()
                .WithConversion(valueConverter, localizationId);
        }

        public static MvxFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<TTarget, TSource, TFrom, TTo>(
            this MvxFluentBindingDescription<TTarget, TSource> bindingDescription,
            IDictionary<TFrom, TTo> converterParameter)
            where TTarget : class
            => bindingDescription.WithConversion(new MvxDictionaryValueConverter<TFrom, TTo>(), new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(converterParameter, default(TTo), false))
            .OneWay();

        public static MvxFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<TTarget, TSource, TFrom, TTo>(
            this MvxFluentBindingDescription<TTarget, TSource> bindingDescription,
            IDictionary<TFrom, TTo> converterParameter,
            TTo fallback)
            where TTarget : class
            => bindingDescription.WithConversion(new MvxDictionaryValueConverter<TFrom, TTo>(), new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(converterParameter, fallback, true))
            .OneWay();
    }
}
