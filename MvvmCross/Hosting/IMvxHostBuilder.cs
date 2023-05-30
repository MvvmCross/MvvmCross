#nullable enable
using Microsoft.Extensions.DependencyInjection;

namespace MvvmCross.Hosting;

public interface IMvxHostBuilder
{
    Func<IServiceCollection, IServiceProvider>? ConfigureContainer { get; }
    IServiceCollection Services { get; }

    IMvxHost Build();
}
