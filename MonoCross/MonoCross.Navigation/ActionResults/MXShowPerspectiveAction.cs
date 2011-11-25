namespace MonoCross.Navigation.ActionResults
{
    public class MXShowPerspectiveAction : IMXActionResult
    {
        private readonly string _viewPerspective;

        public MXShowPerspectiveAction(string whichPerspective)
        {
            _viewPerspective = whichPerspective;
        }

        public void Perform(IMXContainer container, IMXView fromView, IMXController controller)
        {
            // nothing to do
            container.ShowPerspective(fromView, controller, _viewPerspective);
        }
    }
}