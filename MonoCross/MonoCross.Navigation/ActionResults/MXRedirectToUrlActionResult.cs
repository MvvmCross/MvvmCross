namespace MonoCross.Navigation.ActionResults
{
    public class MXRedirectToUrlActionResult : MXRedirectActionResultBase
    {
        private string _redirectUrl;

        public MXRedirectToUrlActionResult(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
        }

        public override void Perform(IMXContainer container, IMXView fromView, IMXController controller)
        {
            container.Redirect(_redirectUrl);
        }
    }
}