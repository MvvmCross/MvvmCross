// IMvxWinRTView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public interface IMvxWinRTView
        : IMvxView

    {
        void ClearBackStack();
        IMvxViewModel ViewModel { get; set; }
    }

    /*
     * hacked out because of:
     * http://stackoverflow.com/questions/9699215/winrt-xamlparseexception-when-i-inherit-a-page-from-a-generic-base-class
     * 
    public interface IMvxWinRTView<TViewModel>
        : IMvxView<TViewModel>
        , IMvxWinRTView
        where TViewModel : class, IMvxViewModel
    {
    }
     */
}