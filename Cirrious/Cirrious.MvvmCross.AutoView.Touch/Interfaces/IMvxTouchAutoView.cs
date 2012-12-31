#region Copyright

// <copyright file="IMvxTouchAutoView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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