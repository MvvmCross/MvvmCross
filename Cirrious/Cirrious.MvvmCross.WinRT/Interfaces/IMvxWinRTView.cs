#region Copyright
// <copyright file="IMvxWinRTView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.WinRT.Interfaces
{
    public interface IMvxWinRTView
        : IMvxView
        , IMvxServiceConsumer<IMvxViewModelLoader>
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