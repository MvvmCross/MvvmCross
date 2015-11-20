// IMvxAutoView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxAutoView
        : IMvxView
    {
    }

    public interface IMvxAutoView<TViewModel>
        : IMvxAutoView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}