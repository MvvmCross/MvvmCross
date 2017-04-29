using Foundation;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public interface IMvxIosViewSegue
    {
        object PrepareViewModelParametersForSegue(UIStoryboardSegue segue, NSObject sender);
    }
}