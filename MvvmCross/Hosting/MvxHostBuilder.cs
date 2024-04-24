#nullable enable
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MvvmCross.Extensions;

namespace MvvmCross.Hosting;

public class MvxHostBuilder : IMvxHostBuilder
{
    public Func<IServiceCollection, IServiceProvider> ConfigureContainer { get; }
    public IServiceCollection Services { get; }

    public MvxHostBuilder(
        IServiceCollection? serviceCollection = null,
        Func<IServiceCollection, IServiceProvider>? configureContainer = null)
    {
        Services = serviceCollection ?? new ServiceCollection();
        ConfigureContainer = configureContainer ?? (s => s.BuildServiceProvider());
    }

    public virtual IMvxHost Build()
    {
        ConfigureDefaultNullLogging(Services);
        AddMvvmCrossServices(Services);
        var serviceCollection = ConfigureContainer(Services);
        var loggerFactory = serviceCollection.GetRequiredService<ILoggerFactory>();

        var host = new MvxHost(serviceCollection, loggerFactory);

        host.Run();

        return host;
    }
    
    protected virtual void AddMvvmCrossServices(IServiceCollection services)
    {
        services.AddCoreServices();
    }

    private static void ConfigureDefaultNullLogging(IServiceCollection services)
    {
        services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
    }
}
