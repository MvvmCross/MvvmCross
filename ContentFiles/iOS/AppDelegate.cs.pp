using Foundation;
using MvvmCross.Platforms.Ios.Core;

namespace $rootnamespace$
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<MvxIosSetup<Core.App>, Core.App>
    {
    }
}
