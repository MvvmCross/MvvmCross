using UIKit;
using MvvmCross.Platform.Tvos.Core;
using MvvmCross.ViewModels;
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

        protected override IMvxTvosViewPresenter CreateViewPresenter()
        {
            return new MvxTvosViewPresenter(ApplicationDelegate, Window);
        }
    }
}
