namespace MonoCross.Navigation.ActionResults
{
    public class MXShowPerspectiveAction<T> : IMXActionResult
    {
        private readonly T _viewModel;
        private readonly string _viewPerspective;

        public MXShowPerspectiveAction(T viewModel, string whichPerspective)
        {
            _viewModel = viewModel;
            _viewPerspective = whichPerspective;
        }

        public void Perform(IMXContainer container, IMXView fromView, IMXController controller)
        {
            // nothing to do
            var viewPerspective = new MXViewPerspective<T>(_viewPerspective);
            var request = new MXShowViewRequest<T>(viewPerspective, _viewModel);
            container.ShowPerspective(fromView, controller, request);
        }
    }
}