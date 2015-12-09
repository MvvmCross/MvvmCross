// MvxSimpleTouchDialogViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Touch.Simple
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