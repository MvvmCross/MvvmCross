// IMvxTouchAutoView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace Cirrious.MvvmCross.AutoView.Touch.Interfaces
{
    public interface IMvxTouchAutoView<TViewModel>
        : IMvxTouchView<TViewModel>
          , IMvxAutoView
          , IMvxBindingViewController
        where TViewModel : class, IMvxViewModel
    {
    }
}