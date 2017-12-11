using MvvmCross.Binding.Binders;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using System.Collections.Generic;

namespace MvvmCross.Binding.BindingContext
{
    public static class MvxFluentBindingDescriptionExtensions
    {
        public static MvxFluentBindingDescription<TTarget, TSource> ToLocalizationId<TTarget, TSource>(
            this MvxFluentBindingDescription<TTarget, TSource> bindingDescription, string localizationId)
            where TSource : IMvxLocalizedTextSourceOwner where TTarget : class
        {
            var valueConverter = Mvx.Resolve<IMvxValueConverterLookup>().Find("Language");
            return bindingDescription.To(vm => vm.LocalizedTextSource)
                .OneTime()
                .WithConversion(valueConverter, localizationId);
        }

        public static MvxFluentBindingDescription<TTarget, TSource> WithDictionaryConversion<TTarget, TSource, TFrom, TTo>(
            this MvxFluentBindingDescription<TTarget, TSource> bindingDescription, IDictionary<TFrom, TTo> converterParameter)
            where TTarget : class
            => bindingDescription.WithConversion(new MvxDictionaryValueConverter<TFrom, TTo>(), converterParameter).OneWay();
    }
}
