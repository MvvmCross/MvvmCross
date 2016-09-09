namespace MvvmCross.iOS.Support.Sidebar
{
    using Platform;
    using iOS.Views.Presenters;
    using MvvmCross.Platform.Platform;
    using UIKit;
    using MvvmCross.Core.ViewModels;
    using JASidePanels;

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
            //return new MvxSidebarPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
            return new MvxSidePanelsPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
        }
	}
}
