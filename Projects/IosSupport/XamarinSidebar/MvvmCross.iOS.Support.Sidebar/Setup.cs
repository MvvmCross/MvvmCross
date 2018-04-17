using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Support.XamarinSidebar;
using MvvmCross.iOS.Support.XamarinSidebarSample.Core;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS
{
    public class Setup : MvxIosSetup
    {
        /// <summary>Initializes a new instance of the <see cref="Setup"/> class.</summary>
        /// <param name="applicationDelegate">The application delegate.</param>
        /// <param name="window">The window.</param>
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected Setup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter) : base(applicationDelegate, presenter)
        {
        }

        /// <summary>Creates the application.</summary>
        /// <returns>The IMvxApplication <see langword="object"/></returns>
        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        /// <summary>Creates the debug trace.</summary>
        /// <returns>The IMvxTrace <see langword="object"/></returns>
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            return new MvxSidebarPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
        }
    }
}
