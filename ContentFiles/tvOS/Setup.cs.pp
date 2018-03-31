using MvvmCross.Platforms.Tvos.Core;
using MvvmCross.Platforms.Tvos.Presenters;
using MvvmCross.ViewModels;
using UIKit;

namespace $rootnamespace$
{
    public class Setup : MvxTvosSetup
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
            return new Core.App();
        }

        protected override IMvxTvosViewPresenter CreatePresenter()
        {
            return new MvxTvosViewPresenter(ApplicationDelegate, Window);
        }
    }
}
