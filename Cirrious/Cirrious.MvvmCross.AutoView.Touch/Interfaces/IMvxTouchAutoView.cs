// IMvxTouchAutoView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.AutoView.Touch.Interfaces
{
    public interface IMvxTouchAutoView
        : IMvxTouchView
          , IMvxAutoView
    {
    }

    public interface IMvxTouchAutoView<TViewModel>
        : IMvxTouchAutoView
          , IMvxTouchView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        new TViewModel ViewModel { get; set; }
    }
}