// MvxBaseIosViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views.Presenters
{
    using System;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class MvxBaseIosViewPresenter
        : MvxViewPresenter, IMvxIosViewPresenter
    {
        public override void Show(MvxViewModelRequest request)
        {
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (this.HandlePresentationChange(hint)) return;

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public virtual bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            return false;
        }

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {
        }

        public override void Close(IMvxViewModel toClose)
        {
        }
    }
}