using Foundation;
using MvvmCross.Platforms.Mac.Core;

namespace $rootnamespace$
{
    [Register("AppDelegate")]
	public class AppDelegate: MvxApplicationDelegate<MvxMacSetup<Core.App>, Core.App>
    {
		public override void DidFinishLaunching(NSNotification notification)
		{
			MvxMacSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();
			RunAppStart();
		}
	}
}
