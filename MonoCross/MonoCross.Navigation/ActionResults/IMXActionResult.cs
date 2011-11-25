namespace MonoCross.Navigation.ActionResults
{
    public interface IMXActionResult
    {
        void Perform(IMXContainer container, IMXView fromView, IMXController controller);
    }
}