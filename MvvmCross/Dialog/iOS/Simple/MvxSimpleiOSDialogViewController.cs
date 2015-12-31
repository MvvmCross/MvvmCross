// MvxSimpleTouchDialogViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.iOS.Simple
{
    using MvvmCross.Core.ViewModels;

    using UIKit;

    public abstract class MvxSimpleTouchDialogViewController : MvxDialogViewController
    {
        protected MvxSimpleTouchDialogViewController()
            : base(UITableViewStyle.Grouped, null, false)
        {
            this.ViewModel = new MvxNullViewModel();
        }
    }
}