using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Platform;
using MonoCross.Touch;
using MonoCross.Navigation;
using MonoTouch.UIKit;

namespace Cirrious.MonoCross.Extensions.Touch
{
    public class MXViewModelTouchContainer : MXTouchContainer
    {
        private MXViewModelLifeCycleHelper _viewModelLifeCycleHelper;

        public new static void Initialize(MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window)
        {
			Initialize(new MXViewModelTouchContainer(theApp, appDelegate, window));
        }

        private MXViewModelTouchContainer(MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window)
            : base(theApp, appDelegate, window)
        {
            _viewModelLifeCycleHelper = new MXViewModelLifeCycleHelper(() => new MXUIMainThreadDispatcher());
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            _viewModelLifeCycleHelper.OnControllerLoadComplete(controller);
            base.OnControllerLoadComplete(fromView, controller, viewPerspective);
        }
    }
}
