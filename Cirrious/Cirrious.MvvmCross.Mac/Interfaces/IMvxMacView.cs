// IMvxMacView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Mac.Interfaces
{
    public interface IMvxMacView
        : IMvxView
    {
        MvxShowViewModelRequest ShowRequest { get; set; }
        IMvxViewModel ViewModel { get; set; }
    }

/*
    public interface IMvxMacView<TViewModel>
        : IMvxView<TViewModel>
        , IMvxMacView
        <IMvxViewModelLoader>
        where TViewModel : class, IMvxViewModel
    {
    }
*/
}