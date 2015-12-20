// IMvxTouchViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views.Presenters
{
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Touch.Views;

    public interface IMvxTouchViewPresenter
        : IMvxViewPresenter
        , IMvxCanCreateTouchView
        , IMvxTouchModalHost
    {
    }
}