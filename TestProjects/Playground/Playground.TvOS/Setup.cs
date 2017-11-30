using UIKit;
using Foundation;

using MvvmCross.Core.ViewModels;
using MvvmCross.tvOS.Platform;
using MvvmCross.tvOS.Views.Presenters;
using MvvmCross.Platform.Platform;

using Playground.Core;

namespace Playground.TvOS
{
    public class Setup : MvxTvosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window) 
            : base(applicationDelegate, window)
        {
        }

        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxTvosViewPresenter presenter) 
            : base(applicationDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
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
