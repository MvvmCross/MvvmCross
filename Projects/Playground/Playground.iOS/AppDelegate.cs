using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Ios.Core;

namespace Playground.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the
// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
[Register("AppDelegate")]
[RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
public class AppDelegate : MvxSceneApplicationDelegate;
