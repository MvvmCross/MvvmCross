using MvvmCross.Platform.Ios.Core;
using MvvmCross.ViewModels;
using Playground.Core;
using UIKit;

namespace Playground.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxIosViewPresenter CreateViewPresenter()
        {
            return new MvxIosViewPresenter(ApplicationDelegate, Window);

        }
    }
}
