namespace MvvmCross.iOS.Support.XamarinSidebarSample.iOS
{
    using Platform;
    using MvvmCross.Platform.Platform;
    using UIKit;
    using MvvmCross.Core.ViewModels;
    using XamarinSidebar;
    using MvvmCross.iOS.Views.Presenters;

    public class Setup : MvxIosSetup
    {
        /// <summary>Initializes a new instance of the <see cref="Setup"/> class.</summary>
        /// <param name="applicationDelegate">The application delegate.</param>
        /// <param name="window">The window.</param>
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        /// <summary>C reates the application.</summary>
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
            return new MvxSidebarPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
        }
    }
}
