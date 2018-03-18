using AppKit;
using Foundation;
using MvvmCross.Forms.Platform.Mac.Core;
using Playground.Forms.UI;
using Xamarin.Forms.Platform.MacOS;

namespace Playground.Forms.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxFormsApplicationDelegate<MvxFormsMacSetup<Core.App, FormsApp>, Core.App, FormsApp>
    {
    }
}
