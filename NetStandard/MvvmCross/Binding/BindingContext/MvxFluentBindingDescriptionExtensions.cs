using MvvmCross.Binding.Binders;
using MvvmCross.Localization;
using MvvmCross.Platform;

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
    }
}
