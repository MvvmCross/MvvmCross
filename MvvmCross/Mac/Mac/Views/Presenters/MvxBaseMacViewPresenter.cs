// MvxBaseMacViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Mac.Views.Presenters
{
    public class MvxBaseMacViewPresenter
        : MvxViewPresenter, IMvxMacViewPresenter
    {
        public override void Show(MvxViewModelRequest request)
        {
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {
        }

        public override void Close(IMvxViewModel toClose)
        {
        }
    }
}