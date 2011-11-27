namespace MonoCross.Navigation.ActionResults
{
    public abstract class MXRedirectActionResultBase : IMXActionResult
    {
        // currently this class adds no value whatsoever... but it does make the inheritance tree look nice!
        // keep this class for now - just in case there is future common functionality here
        public abstract void Perform(IMXContainer container, IMXView fromView, IMXController controller);
    }
}