using System.Windows.Threading;
using Cirrious.MonoCross.Extensions.Platform;
using Microsoft.Phone.Controls;
using MonoCross.Navigation;
using MonoCross.WindowsPhone;

namespace Cirrious.MonoCross.Extensions.WindowsPhone
{
    public class MXViewModelPhoneContainer : MXPhoneContainer
    {
        private readonly MXViewModelLifeCycleHelper _viewModelLifeCycleHelper;

        public new static void Initialize(MXApplication theApp, PhoneApplicationFrame rootFrame)
        {
            MXContainer.InitializeContainer(new MXViewModelPhoneContainer(theApp, rootFrame));
        }

        public MXViewModelPhoneContainer(MXApplication theApp, PhoneApplicationFrame rootFrame)
            : base(theApp, rootFrame)
        {
            var dispatcher = rootFrame.Dispatcher;
            _viewModelLifeCycleHelper = new MXViewModelLifeCycleHelper(() => new MXPhoneViewDispatcher(dispatcher));
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            _viewModelLifeCycleHelper.OnControllerLoadComplete(controller);
            base.OnControllerLoadComplete(fromView, controller, viewPerspective);
        }
    }
}
