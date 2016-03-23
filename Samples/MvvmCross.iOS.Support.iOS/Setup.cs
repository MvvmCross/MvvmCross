using MvvmCross.iOS.Support.JASidePanels;

namespace MvvmCross.iOS.Support.iOS
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.iOS.Views.Presenters;
    using MvvmCross.Platform.Platform;
    using Platform;
    using UIKit;

    public class Setup : MvxIosSetup
    {
        /// <summary>Initializes a new instance of the <see cref="Setup"/> class.</summary>
        /// <param name="applicationDelegate">The application delegate.</param>
        /// <param name="window">The window.</param>
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        /// <summary>Creates the application.</summary>
        /// <returns>The IMvxApplication <see langword="object"/></returns>
        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        /// <summary>Creates the debug trace.</summary>
        /// <returns>The IMvxTrace <see langword="object"/></returns>
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            return new MvxSidePanelsPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
        }
    }
}