// IMvxIosViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views.Presenters
{
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.tvOS.Views;

    public interface IMvxIosViewPresenter
        : IMvxViewPresenter
        , IMvxCanCreateIosView
        , IMvxIosModalHost
    {
    }
}