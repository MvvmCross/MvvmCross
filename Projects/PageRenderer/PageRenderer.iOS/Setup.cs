using UIKit;
using MvvmCross.iOS.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.iOS;

namespace PageRendererExample.UI.iOS
{
    public class Setup : MvxFormsIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.LazyConstructAndRegisterSingleton<IImageHolder, ImageHolder>();
            return new CoreApp();
        }

        protected override MvxFormsApplication CreateFormsApplication()
        {
            return new PageRendererExampleApp();
        }
    }
}