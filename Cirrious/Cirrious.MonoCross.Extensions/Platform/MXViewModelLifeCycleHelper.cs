using System;
using Cirrious.MonoCross.Extensions.Interfaces;
using MonoCross.Navigation;

namespace Cirrious.MonoCross.Extensions.Platform
{
    public class MXViewModelLifeCycleHelper
    {
        private readonly Func<IMXViewDispatcher> _dispatcherFactory;
        private IMXViewModel _lastSeenViewModel;

        public MXViewModelLifeCycleHelper(Func<IMXViewDispatcher> dispatcherFactory)
        {
            _dispatcherFactory = dispatcherFactory;
        }

        public void OnControllerLoadComplete(IMXController controller)
        {
            if (_lastSeenViewModel != null)
            {
                // TODO - this requestStop is possibly called too late... but not sure we have any 
                _lastSeenViewModel.RequestStop();
                _lastSeenViewModel = null;
            }

            var model = controller.GetModel();
            _lastSeenViewModel = model as IMXViewModel;
            if (_lastSeenViewModel != null)
            {
                _lastSeenViewModel.ViewDispatcher = _dispatcherFactory();
            }
        }
    }
}