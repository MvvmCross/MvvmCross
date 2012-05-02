#region Copyright
// <copyright file="IMvxWindowsPhoneView.cs" company="Cirrious">
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

namespace Cirrious.MvvmCross.WindowsPhone.Interfaces
{
    public interface IMvxWindowsPhoneView
        : IMvxView
        , IMvxServiceConsumer<IMvxWindowsPhoneViewModelRequestTranslator>
        , IMvxServiceConsumer<IMvxViewModelLoader>
    {
        void ClearBackStack();
    }

    public interface IMvxWindowsPhoneView<TViewModel>
        : IMvxView<TViewModel>
        , IMvxWindowsPhoneView
        where TViewModel : class, IMvxViewModel
    {
    }
}