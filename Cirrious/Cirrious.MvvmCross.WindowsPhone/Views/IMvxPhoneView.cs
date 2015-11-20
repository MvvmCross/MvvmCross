// IMvxPhoneView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public interface IMvxPhoneView
        : IMvxView
    {
        void ClearBackStack();
    }

    public interface IMvxPhoneView<TViewModel>
        : IMvxPhoneView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}