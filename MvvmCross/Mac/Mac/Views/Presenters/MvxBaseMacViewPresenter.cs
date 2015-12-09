// MvxBaseMacViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Mac.Views.Presenters
{
    using global::MvvmCross.Core.ViewModels;
    using global::MvvmCross.Core.Views;
    using global::MvvmCross.Platform.Platform;

    public class MvxBaseMacViewPresenter
        : MvxViewPresenter, IMvxMacViewPresenter
    {
        public override void Show(MvxViewModelRequest request)
        {
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (this.HandlePresentationChange(hint)) return;

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {
        }
    }
}