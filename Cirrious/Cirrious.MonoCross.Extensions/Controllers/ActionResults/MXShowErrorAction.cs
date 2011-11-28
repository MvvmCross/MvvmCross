using System;

namespace MonoCross.Navigation.ActionResults
{
    public class MXShowErrorAction : IMXActionResult
    {
        private Exception _exception;

        public MXShowErrorAction(Exception exception)
        {
            _exception = exception;
        }

        public void Perform(IMXContainer container, IMXView fromView, IMXController controller)
        {
            // nothing to do
            container.ShowError(fromView, controller, _exception);
        }
    }
}