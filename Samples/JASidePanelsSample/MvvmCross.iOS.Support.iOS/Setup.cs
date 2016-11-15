namespace MvvmCross.iOS.Support.JASidePanelsSample.iOS
{
    using JASidePanels;
    using Platform;
    using MvvmCross.iOS.Views.Presenters;
    using MvvmCross.Platform.Platform;
    using UIKit;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.iOS.Support.JASidePanelsSample.Core;


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
            return new App();
        }

        /// <summary>Creates the debug trace.</summary>
        /// <returns>The IMvxTrace <see langword="object"/></returns>
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        /// <summary>
        /// Creates the presenter.
        /// </summary>
        /// <returns></returns>
        protected override IMvxIosViewPresenter CreatePresenter()
        {
            return new MvxSidePanelsPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
        }
    }
}