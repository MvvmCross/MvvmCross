namespace MvvmCross.iOS.Views
{

    using Foundation;
    using UIKit;

    public interface IMvxIosViewSegue
    {
        object PrepareViewModelParametersForSegue(UIStoryboardSegue segue, NSObject sender);
    }
}