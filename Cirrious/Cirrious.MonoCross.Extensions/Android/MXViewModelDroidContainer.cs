using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Platform;
using MonoCross.Droid;
using MonoCross.Navigation;
using Android.Content;

namespace Cirrious.MonoCross.Extensions.Android
{
    public class MXViewModelDroidContainer : MXDroidContainer
    {
        private  MXViewModelLifeCycleHelper _viewModelLifeCycleHelper;

        public new static void Initialize(MXApplication theApp, Context applicationContext)
        {
            MXContainer.InitializeContainer(new MXViewModelDroidContainer(theApp, applicationContext));
        }

        public MXViewModelDroidContainer(MXApplication theApp, Context applicationContext)
            : base(theApp, applicationContext)
        {
            _viewModelLifeCycleHelper = new MXViewModelLifeCycleHelper(() => new MXSimpleDispatcher());
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            _viewModelLifeCycleHelper.OnControllerLoadComplete(controller);
            base.OnControllerLoadComplete(fromView, controller, viewPerspective);
        }
    }
}
