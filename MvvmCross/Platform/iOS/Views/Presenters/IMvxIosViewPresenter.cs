// IMvxIosViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.Views;
using UIKit;

namespace MvvmCross.iOS.Views.Presenters
{
    public interface IMvxIosViewPresenter : IMvxViewPresenter, IMvxCanCreateIosView
    {
        void ShowModalViewController(UIViewController viewController, bool animated);

        void CloseModalViewController(UIViewController viewController);

        void CloseModalViewControllers();

        UIViewController GetTopViewController();
    }
}
