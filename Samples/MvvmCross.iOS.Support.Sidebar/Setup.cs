using MvvmCross.iOS.Support.XamarinSidebar;


namespace MvvmCross.iOS.Support.Sidebar
{
	using MvvmCross.Platform;
    using MvvmCross.iOS.Platform;
	using MvvmCross.iOS.Views.Presenters;
    using MvvmCross.iOS.Support.SidePanels;
	using MvvmCross.Platform.Platform;
	using UIKit;
	using MvvmCross.Core.ViewModels;


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
			var presenter = new MvxSidebarPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);

            if(presenter is IMvxSideMenu)
                Mvx.RegisterSingleton<IMvxSideMenu>(presenter);

            return presenter;
		}
	}
}
