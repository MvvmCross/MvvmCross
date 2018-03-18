using Foundation;
using MvvmCross;
using UIKit;
using MvvmCross.Platform.Tvos.Core;
using MvvmCross.ViewModels;
using Playground.Core;

namespace Playground.TvOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<MvxTvosSetup<App>, App>
    {
    }
}
