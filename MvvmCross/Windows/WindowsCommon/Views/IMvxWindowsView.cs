// IMvxStoreView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsCommon.Views
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public interface IMvxWindowsView
        : IMvxView
    {
        void ClearBackStack();
    }

    public interface IMvxWindowsView<TViewModel>
        : IMvxWindowsView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}