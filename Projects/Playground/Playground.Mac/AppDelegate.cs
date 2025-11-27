using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using Playground.Core;

namespace Playground.Mac
{
    [Register("AppDelegate")]
    [RequiresUnreferencedCode("MvxApplicationDelegate requires unreferenced code")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    {
        public AppDelegate()
        {
            MvxWindowPresentationAttribute.DefaultWidth = 250;
            MvxWindowPresentationAttribute.DefaultHeight = 250;
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
