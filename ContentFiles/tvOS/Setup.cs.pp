using MvvmCross.Core.ViewModels;
using MvvmCross.tvOS.Platform;
using MvvmCross.tvOS.Views.Presenters;
using MvvmCross.Platform.Platform;
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
        
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxTvosViewPresenter CreatePresenter()
        {
            return new MvxTvosViewPresenter(ApplicationDelegate, Window);
        }
    }
}
