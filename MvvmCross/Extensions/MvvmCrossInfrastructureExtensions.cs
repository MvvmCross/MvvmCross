using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Commands;
using MvvmCross.Core;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MvvmCross.Extensions;

public static class MvvmCrossInfrastructureExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IMvxSettings, MvxSettings>();
        services.AddSingleton<IMvxStringToTypeParser, MvxStringToTypeParser>();
        services.AddSingleton<IMvxViewModelLoader, MvxViewModelLoader>();
        services.AddSingleton<IMvxNavigationService, MvxNavigationService>();
        services.AddSingleton<MvxViewModelByNameLookup>();
        services.AddSingleton<IMvxViewModelTypeFinder, MvxViewModelViewTypeFinder>();
        services.AddSingleton<IMvxTypeToTypeLookupBuilder, MvxViewModelViewLookupBuilder>();
        services.AddSingleton<IMvxCommandCollectionBuilder, MvxCommandCollectionBuilder>();
        services.AddSingleton<IMvxNavigationSerializer, MvxStringDictionaryNavigationSerializer>();
        services.AddSingleton<IMvxChildViewModelCache, MvxChildViewModelCache>();

        services.AddTransient<IMvxCommandHelper, MvxWeakCommandHelper>();
        return services;
    }
}
