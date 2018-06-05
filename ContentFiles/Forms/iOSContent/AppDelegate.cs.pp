using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;

namespace $rootnamespace$
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate<MvxFormsIosSetup<Core.App, FormsUI.App>, Core.App, FormsUI.App>
    {
    }
}
