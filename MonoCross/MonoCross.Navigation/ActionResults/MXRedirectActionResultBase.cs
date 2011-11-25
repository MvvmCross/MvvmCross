namespace MonoCross.Navigation.ActionResults
{
    public abstract class MXRedirectActionResultBase : IMXActionResult
    {
        public abstract void Perform(IMXContainer container, IMXView fromView, IMXController controller);
    }
}