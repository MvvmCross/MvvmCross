namespace Eventhooks.iOS
{
	using Eventhooks.Core;
	using MvvmCross.Binding.Bindings.Target.Construction;
	using MvvmCross.Core.ViewModels;
	using MvvmCross.iOS.Platform;
	using MvvmCross.iOS.Views.Presenters;
	using MvvmCross.Platform.Platform;
	using UIKit;

	public class Setup : MvxIosSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}

		public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
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
	}
}