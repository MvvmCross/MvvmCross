using MvvmCross.Platforms.Ios.Core;
using Playground.Core;

namespace Playground.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the
// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
[Register("AppDelegate")]
public partial class AppDelegate : MvxApplicationDelegate<Setup, App>;
