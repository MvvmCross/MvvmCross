using Foundation;
using MvvmCross.Platforms.Tvos.Core;

namespace $rootnamespace$
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<MvxTvosSetup<Core.App>, Core.App>
    {
    }
}
