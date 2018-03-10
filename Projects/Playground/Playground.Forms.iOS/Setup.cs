using MvvmCross.Forms.Platform.Ios.Core;
using MvvmCross.Platform.Ios.Core;
using Playground.Forms.UI;
using UIKit;

namespace Playground.Forms.iOS
{
    public class Setup : MvxFormsIosSetup<Core.App, FormsApp>
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }
    }
}
