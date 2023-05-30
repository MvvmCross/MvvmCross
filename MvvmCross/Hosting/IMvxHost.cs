#nullable enable
using Microsoft.Extensions.Logging;

namespace MvvmCross.Hosting;

public interface IMvxHost : IDisposable
{
    IServiceProvider Services { get; }
    ILoggerFactory Logging { get; }

    void Run();
}
