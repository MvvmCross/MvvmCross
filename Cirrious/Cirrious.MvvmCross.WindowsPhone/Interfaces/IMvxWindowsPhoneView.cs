// IMvxWindowsPhoneView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.WindowsPhone.Interfaces
{
    public interface IMvxWindowsPhoneView
        : IMvxView
          , IMvxServiceConsumer
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