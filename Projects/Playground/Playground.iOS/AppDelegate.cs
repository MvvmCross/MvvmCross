using MvvmCross.Platforms.Ios.Core;
using Serilog;

namespace Playground.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the
// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
[Register("AppDelegate")]
public class AppDelegate : MvxSceneApplicationDelegate
{
    public override void FinishedLaunching(UIApplication application)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Async(a => a.NSLog())
            .WriteTo.Async(a => a.Trace())
            .CreateLogger();
        base.FinishedLaunching(application);
    }
}
