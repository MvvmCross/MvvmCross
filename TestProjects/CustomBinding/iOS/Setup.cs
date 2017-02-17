using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.Platform;
using MvvmCross.TestProjects.CustomBinding.Core;
using MvvmCross.TestProjects.CustomBinding.iOS.Bindings;
using MvvmCross.TestProjects.CustomBinding.iOS.Controls;
using UIKit;

namespace MvvmCross.TestProjects.CustomBinding.iOS
{
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

		protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
		{
			registry.RegisterCustomBindingFactory<BinaryEdit>(
				"MyCount", 
				(arg) => new BinaryEditTargetBinding(arg));
			
			base.FillTargetFactories(registry);
		}
    }
}
